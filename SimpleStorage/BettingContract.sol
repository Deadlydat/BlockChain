// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.7.0 < 0.9.0;

import "@openzeppelin/contracts/utils/math/SafeMath.sol";
import "./ERC173.sol";

contract Betting is ERC173 {
   using SafeMath for uint256;
   
   struct Player {
      uint256 betAmount;
      bytes32 hashOfTeam;
   }

   enum GameStatus {
        NOT_IN_PROGRESS,
        IN_PROGRESS,
        FINISHED
   }
   
   address payable private _owner;
   uint256 private _moneyPool;
   address payable[] private _players;
   GameStatus private _gameStatus;
   
   mapping(address => Player) private _playerInfo;
   
   
   constructor() {
      _owner = payable(msg.sender);
      _gameStatus = GameStatus.NOT_IN_PROGRESS;
   }

   modifier onlyOwner() {
      require(_owner == msg.sender, "Only the owner is allowed to call that function!");
      _;
   }

   function getPlayerCount() public view returns(uint256) {
      return _players.length;
   }
   
   function kill() onlyOwner public {
      selfdestruct(_owner);
   }

   function giveMoneyBack() onlyOwner public {
      for(uint256 i = 0; i < _players.length; i++) {
         address payable playerAddress = _players[i];
         uint256 betAmountToGiveBack = _playerInfo[playerAddress].betAmount;
         playerAddress.transfer(betAmountToGiveBack);
      }

      resetGameInformation();
   }
    
   function checkIfPlayerExists(address payable player) private view returns(bool) {
      for(uint256 i = 0; i < _players.length; i++){
         if(_players[i] == player) {
            return true;
         }
      }
      return false;
   }

   function bet(string memory teamRepresentation) public payable {
      require(_gameStatus == GameStatus.NOT_IN_PROGRESS, "You can only bet before the game started!");
      require(payable(msg.sender) != _owner, "The owner is not allowed to bet!");
      require(msg.value != 0, "Betting amount shouldn't be 0!");
      require(!checkIfPlayerExists(payable(msg.sender)), "Player already exists!");

      _playerInfo[msg.sender].betAmount = msg.value;
      _playerInfo[msg.sender].hashOfTeam = keccak256(abi.encodePacked(teamRepresentation));
      
      _players.push(payable(msg.sender));
      
      _moneyPool = _moneyPool.add(msg.value);
   }

   function startGame() onlyOwner public {
      require(_gameStatus != GameStatus.IN_PROGRESS, "There is currently a game in progress!");
      require(_gameStatus != GameStatus.FINISHED, "The game is already finished!");
      require(getPlayerCount() != 0, "Can't start a game without players!");
      _gameStatus = GameStatus.IN_PROGRESS;
   }

   function finishGame() onlyOwner public {
     require(_gameStatus == GameStatus.IN_PROGRESS, "There is currently no game in progress!");
     _gameStatus = GameStatus.FINISHED;
   }

   function distributePrizes(string[] memory teamRepresentationOfWinners) onlyOwner public {
      require(_gameStatus != GameStatus.IN_PROGRESS, "You can't distribute the prizes when a game is in progress!");
      require(_gameStatus != GameStatus.NOT_IN_PROGRESS, "You can't distribute the prizes before a game has started!");

      address payable[100] memory winners;
      uint256 winnerCount = 0;
      uint256 winnersInvest = 0;
      address payable playerAddress;

      for(uint256 i = 0; i < teamRepresentationOfWinners.length; i++) {
         bytes32 winnerHash = keccak256(abi.encodePacked(teamRepresentationOfWinners[i]));

         for(uint256 j = 0; j < _players.length; j++) {
            playerAddress = _players[j];

            if(_playerInfo[playerAddress].hashOfTeam == winnerHash) {
               winners[winnerCount] = playerAddress;
               winnerCount++;
               winnersInvest = winnersInvest.add(_playerInfo[playerAddress].betAmount);
            }
         }
      }

      require(winnerCount != 0, "There must be at least one winner!");
      require(winnersInvest != 0, "You can only win if you bet with money!");

      uint256 transferedMoney = 0;

      for(uint256 i = 0; i < winnerCount; i++){
         address payable winner = winners[i];
         // Check that the address in this fixed array is not empty
         if(winner != address(0)) {
            uint256 winnerBetAmount = _playerInfo[winner].betAmount;
            
            /*
               We calculate the prize money that each winner gets by multiplying the betAmount with 
               the total money pool and we divide it by the total invest of all winners.
               We do it in this particular order to avoid as much cutoff of floating point values
               as possible to ensure that we payout as much as possible.
            */
            uint256 prizeMoney = winnerBetAmount.mul(_moneyPool).div(winnersInvest);
            winner.transfer(prizeMoney);
            transferedMoney = transferedMoney.add(prizeMoney);
         }
      }

      _moneyPool = _moneyPool.sub(transferedMoney);
      resetGameInformation();
   }

   // Resets the game information so that the contract can be reused for further bets.
   // The rest of the moneyPool is transfered to us.
   function resetGameInformation() private {
      _gameStatus = GameStatus.NOT_IN_PROGRESS;
      _owner.transfer(_moneyPool);
      _moneyPool = 0;
      delete _players;
   }

   function getMoneyPool() public view returns(uint256){
      return _moneyPool;
   }
    
    /// @notice Get the address of the owner    
    /// @return The address of the owner.
   function owner() view external override returns(address) {
      return _owner;
   }
	
    /// @notice Set the address of the new owner of the contract
    /// @dev Set newOwner to address(0) to renounce any ownership.
    /// @param newOwner The address of the new owner of the contract    
    function transferOwnership(address newOwner) public virtual override onlyOwner {
        require(newOwner != address(0), "Ownable: new owner is the zero address");
        address oldOwner = _owner;
        _owner = payable(newOwner);
        emit OwnershipTransferred(oldOwner, newOwner);
    }

   

}