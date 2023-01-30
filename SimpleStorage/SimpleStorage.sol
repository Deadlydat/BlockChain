// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.7.0 < 0.9.0;

/*
Next steps:
1 - Is the reset sufficient?
2 - Maybe use a time stamp
3 - Variable bet amount?
4 - Multiple winners
*/

contract Betting {
   
   struct Player {
      uint256 amountBet;
      bytes32 hashOfTeam;
   }

   enum GameStatus {
        NOT_IN_PROGRESS,
        IN_PROGRESS,
        FINISHED
   }
   
   address payable public owner;
   uint256 public betAmount = 100000000000000;
   uint256 public moneyPool;
   address payable[] public players;
   //uint256 public expiration;
   GameStatus gameStatus;
   
   mapping(address => Player) public playerInfo;
   
   
   constructor() {
      owner = payable(msg.sender);
      gameStatus = GameStatus.NOT_IN_PROGRESS;
      //expriation = block.timestamp + duration;
   }

   function getPlayerCount() public view returns(uint256) {
      return players.length;
   }
   
   function kill() public {
      require(msg.sender == owner, "Only the owner can kill the contract!");
      if(msg.sender == owner) {
         selfdestruct(owner);
      } 
   }
    
   function checkPlayerExists(address payable player) private view returns(bool){
      for(uint256 i = 0; i < players.length; i++){
         if(players[i] == player) {
            return true;
         }
      }
      return false;
   }

   function bet(string memory teamRepresentation) public payable {
      require(gameStatus == GameStatus.NOT_IN_PROGRESS, "There is currently a game in progress. You can only bet before the game started!");
      require(payable(msg.sender) != owner, "The owner is not allowed to bet!");
      require(!checkPlayerExists(payable(msg.sender)), "Player already exists!");
      require(msg.value == betAmount, "Wrong bet amount. It has to be 100000000000000 Wei!");

      //We set the player informations : amount of the bet and the hash of the selected team
      playerInfo[msg.sender].amountBet = msg.value;
      playerInfo[msg.sender].hashOfTeam = keccak256(abi.encodePacked(teamRepresentation));
      
      //then we add the address of the player to the players array
      players.push(payable(msg.sender));
      
      moneyPool += msg.value;
   }

   function startGame() public {
      require(msg.sender == owner, "Only the owner can start a game!");
      require(gameStatus == GameStatus.NOT_IN_PROGRESS, "There is currently a game in progress!");
      gameStatus = GameStatus.IN_PROGRESS;
   }

   function finishGame() public {
     require(msg.sender == owner, "Only the owner can finish a game!");
     require(gameStatus == GameStatus.IN_PROGRESS, "There is currently no game in progress!");
     gameStatus = GameStatus.FINISHED;
   }

   function distributePrizes(string memory teamRepresentationOfWinner) public {
      require(msg.sender == owner, "Only the owner can finish the game and distribute the prices!");
      require(gameStatus == GameStatus.FINISHED, "There is currently no game in progress!");

      address payable[1000] memory winners;
      uint256 winnerCount = 0; // This is the count for the array of winners
      address payable playerAddress;

      for(uint256 i = 0; i < players.length; i++) {
         playerAddress = players[i];
         
         if(playerInfo[playerAddress].hashOfTeam == keccak256(abi.encodePacked(teamRepresentationOfWinner))) {
            winners[winnerCount] = playerAddress;
            winnerCount++;
         }
      }

      require(winnerCount != 0, "There must be at least one winner!");

      uint prizeMoney = moneyPool / winnerCount;

      for(uint256 j = 0; j < winnerCount; j++){
          // Check that the address in this fixed array is not empty
         if(winners[j] != address(0)) {
            winners[j].transfer(prizeMoney);
         }
      }

      resetGameInformation();
      //selfdestruct(owner);
   }

   function resetGameInformation() private {
      gameStatus = GameStatus.NOT_IN_PROGRESS;
      moneyPool = 0;
      delete players;
      //delete playerInfo[playerAddress]; // Delete all the players
      //delete playerInfo;
   }

   function getMoneyPool() public view returns(uint256){
      return moneyPool;
   }

}