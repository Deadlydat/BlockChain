from eth_utils import address
from web3 import Web3
from solcx import compile_standard, install_solc
from dotenv import load_dotenv
import json   
import logging

load_dotenv()

CONTRACT_ADDRESS = "0x9314658625A4a4c212859Abd15D6A168aE6d6731"
CHAIN_ID = 1337
W3 = Web3(Web3.HTTPProvider("HTTP://192.168.178.27:7545"))
GAS_PRICE = 20000000000


def compile_contract():
    with open("./Betting.sol", "r") as file:
        betting_file = file.read()

        compiled_sol = compile_standard(
            {
                "language": "Solidity",
                "sources": {"Betting.sol": {"content": betting_file}},
                "settings": {
                    "outputSelection": {
                        "*": {
                            "*": ["abi", "metadata", "evm.bytecode", "evm.bytecode.sourceMap"]
                        }
                    }
                },
            },
            solc_version="0.8.7",
        )

    with open("compiled_code.json", "w") as file:
        json.dump(compiled_sol, file)

    bytecode = compiled_sol["contracts"]["Betting.sol"]["Betting"]["evm"]["bytecode"]["object"]
    return json.loads(compiled_sol["contracts"]["Betting.sol"]["Betting"]["metadata"])["output"]["abi"]


# TODO: test
def kill_contract(kill_address, private_key, nonce):
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    kill_tx = betting.functions.kill().buildTransaction(
        {"chainId": CHAIN_ID, "from": kill_address, "nonce": nonce, "gasPrice": GAS_PRICE}
    )
    signed_kill_tx = W3.eth.account.sign_transaction(kill_tx, private_key=private_key)
    send_kill_tx = W3.eth.send_raw_transaction(signed_kill_tx.rawTransaction)
    tx_receipt = W3.eth.wait_for_transaction_receipt(send_kill_tx)
    logging.info(tx_receipt)


# TODO test what the logger says
def bet(team_representation, address_of_better, private_key, value):
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    nonce = W3.eth.getTransactionCount(address_of_better)
    bet_transaction = betting.functions.bet(team_representation).buildTransaction(
        {"chainId": CHAIN_ID, "from": address_of_better, "nonce": nonce, "value": value, "gasPrice": GAS_PRICE}
    )
    signed_bet_tx = W3.eth.account.sign_transaction(bet_transaction, private_key=private_key)
    send_bet_tx = W3.eth.send_raw_transaction(signed_bet_tx.rawTransaction)
    tx_receipt = W3.eth.wait_for_transaction_receipt(send_bet_tx)
    logging.info(tx_receipt)


# TODO: test
def distribute_prices(team_representation, address_of_distributer, private_key):
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    nonce = W3.eth.getTransactionCount(address_of_distributer)
    distribute_tx = betting.functions.distributePrizes(team_representation).buildTransaction(
        {"chainId": CHAIN_ID, "from": address_of_distributer, "nonce": nonce, "gasPrice": GAS_PRICE}
    )
    signed_dist_tx = W3.eth.account.sign_transaction(distribute_tx, private_key=private_key)
    send_dist_tx = W3.eth.send_raw_transaction(signed_dist_tx.rawTransaction)
    tx_receipt = W3.eth.wait_for_transaction_receipt(send_dist_tx)
    logging.info(tx_receipt)


# TODO: test
def start_game(address_of_starter, private_key):
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    nonce = W3.eth.getTransactionCount(address_of_starter)
    start_game_tx = betting.functions.startGame().buildTransaction(
        {"chainId": CHAIN_ID, "from": address_of_starter, "nonce": nonce, "gasPrice": GAS_PRICE}
    )
    signed_start_tx = W3.eth.account.sign_transaction(start_game_tx, private_key=private_key)
    send_start_tx = W3.eth.send_raw_transaction(signed_start_tx.rawTransaction)
    tx_receipt = W3.eth.wait_for_transaction_receipt(send_start_tx)
    logging.info(tx_receipt)


# TODO: test
def finish_game(address_of_finisher, private_key):
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    nonce = W3.eth.getTransactionCount(address_of_finisher)
    finish_game_tx = betting.functions.finishGame().buildTransaction(
        {"chainId": CHAIN_ID, "from": address_of_finisher, "nonce": nonce, "gasPrice": GAS_PRICE}
    )
    signed_finish_tx = W3.eth.account.sign_transaction(finish_game_tx, private_key=private_key)
    send_finish_tx = W3.eth.send_raw_transaction(signed_finish_tx.rawTransaction)
    tx_receipt = W3.eth.wait_for_transaction_receipt(send_finish_tx)
    logging.info(tx_receipt)


def get_money_pool():
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    money_pool = betting.functions.getMoneyPool().call()
    logging.info(money_pool)
    print(money_pool)


def get_player_count():
    betting = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    player_count = betting.functions.getPlayerCount().call()
    logging.info(player_count)
    print(player_count)


def create_account():
    new_account = W3.eth.account.create()

    new_account_address = new_account.address
    private_key = new_account.privateKey.hex()

    print(new_account_address)
    print(private_key)

    account = W3.eth.account.privateKeyToAccount(private_key)
        
    print(account.key.hex())
    print(account.address)

    print(f"account = {account}")

    return new_account_address, private_key


if __name__ == '__main__':
    abi = compile_contract()

    betting_contract = W3.eth.contract(address=CONTRACT_ADDRESS, abi=abi)
    # print(betting_contract.functions.getMoneyPool().call())

    # address, private_key = create_account()
    #
    # bet("BEBE", address, private_key, 100000000000000)
    # finish_game("0x8d7d8ff78903F8902710b5Ea536e3B0D839F248D", "0x840af416508488a7ef3f7e822d71b381235bae6c1286f9203d1d89818a7dcb47")
    # distribute_prices("HAHA", "0x8d7d8ff78903F8902710b5Ea536e3B0D839F248D", "0x840af416508488a7ef3f7e822d71b381235bae6c1286f9203d1d89818a7dcb47")
    # snd_acc_address = os.getenv("ADDRESS_2ND")
    # snd_acc_pk = os.getenv("PRIVET_KEY_2ND")
    #
    # print(snd_acc_address)
    # print(snd_acc_pk)
    # print(os.getenv("BLOCKCHAIN_URL"))

    # create_account()
    #print(W3.eth.accounts)

    # W3.eth.getBalance()
    # print(create_account())
    # print(len(W3.eth.accounts))

    # new_account = W3.geth.personal.new_account("test")
    #
    # W3.eth.account.privateKeyToAccount()
    # print(new_account)

    create_account()

    print(len(W3.eth.accounts))

    # TODO find a good way to generate nonce

    # get_money_pool()
    # get_player_count()
    # bet("HAHA", snd_acc_address, snd_acc_pk, 100000000000000, W3.eth.getTransactionCount(snd_acc_address))




class test:
    def print_hi(self,name):
        print(f'Hi, {name}')
        return name  

    def calculate(self,num1, num2):
        print(num1 + num2)



