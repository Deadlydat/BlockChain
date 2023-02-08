const { expect } = require("chai");
const { ethers } = require("hardhat");

describe("Betting", function () {
    it("moneyPool and playercount should be 0 before someone bet", async function () {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        let moneyPool = (await betting.getMoneyPool()).toNumber();
        let playerCount = (await betting.getPlayerCount()).toNumber();
        expect(moneyPool).to.equal(0);
        expect(playerCount).to.equal(0);
    });
    it("owner shouldn't be able to bet", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner] = await ethers.getSigners();

        await expect(betting.bet("T", {from: owner.address, value: 1}))
            .to.be.revertedWith("revert The owner is not allowed to bet!");
    });
    it("owner() function should return owner", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner] = await ethers.getSigners();

        const ownerFromCall = (await betting.owner.call()).toString();
        expect(owner.address).to.be.equal(ownerFromCall);
    });
    it("Shouldn't be able to start the game without a player.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner] = await ethers.getSigners();

        await expect(betting.startGame({from: owner.address})).to.be.revertedWith("revert Can't start a game without players!");
    });
    it("owner shouldn't be able to bet", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner] = await ethers.getSigners();

        await expect(betting.bet("T", {from: owner.address, value: 1}))
            .to.be.revertedWith("revert The owner is not allowed to bet!");
    });
    it("Shouldn't be able to kill the contract.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        await expect(betting.connect(addr1).kill()).to.be.revertedWith("revert Only the owner is allowed to call that function!");
    });
    it("transferring owner should work", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        const oldOwner = (await betting.owner.call()).toString();

        await betting.transferOwnership(addr1.address);

        const newOwner = (await betting.owner.call()).toString();

        expect(oldOwner).to.equal(owner.address);
        expect(addr1.address).to.equal(newOwner);
    });
    it("Betting should increase playerCount and moneyPool.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        let moneyPoolBefore = (await betting.getMoneyPool.call()).toNumber();
        let playerCountBefore = (await betting.getPlayerCount.call()).toNumber();

        await betting.connect(addr1).bet("Random String", {value: 1});

        let moneyPoolAfter = (await betting.getMoneyPool.call()).toNumber();
        let playerCountAfter = (await betting.getPlayerCount.call()).toNumber();

        expect(moneyPoolBefore + 1).to.equal(moneyPoolAfter);
        expect(playerCountBefore + 1).to.equal(playerCountAfter);
    });
    it("Shouldn't be able to bet after a game has started.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1, addr2] = await ethers.getSigners();

        // working bet
        await betting.connect(addr1).bet("Random String", {value: 1});

        // starting game
        await betting.connect(owner).startGame();

        await expect(betting.connect(addr2).bet("test", {value: 2}))
            .to.be.revertedWith("revert You can only bet before the game started!");
    });
    it("Shouldn't be able to start game twice", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1, addr2] = await ethers.getSigners();

        await betting.connect(addr1).bet("Random String", {value: 1});
        await betting.connect(addr2).bet("Random String 2", {value: 1});
        await betting.connect(owner).startGame();
        await expect(betting.connect(owner).startGame()).to.be.revertedWith("revert There is currently a game in progress!");
    });
    it("Should be able to bet only once", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        await betting.connect(addr1).bet("Random String", {value: 1});

        await expect(betting.connect(addr1).bet("Random String x2", {value: 1}))
            .to.be.revertedWith("revert Player already exists!");
    });
    it("Shouldn't be able to distribute prizes without a started game", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        await betting.connect(addr1).bet("Random String", {value: 1});

        await expect(betting.connect(owner).distributePrizes(["Random String"]))
            .to.be.revertedWith("revert You can't distribute the prizes before a game has started!");
    });
    it("Shouldn't be able to distribute prizes without a empty winners array", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        await betting.connect(addr1).bet("Random String", {value: 1});
        await betting.connect(owner).startGame();
        await betting.finishGame();

        await expect(betting.connect(owner).distributePrizes([]))
            .to.be.revertedWith("revert There must be at least one winner!");
    });
    it("Shouldn't be able to distribute prizes without a finished game.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1] = await ethers.getSigners();

        await betting.connect(addr1).bet("Random String", {value: 1});
        await betting.connect(owner).startGame();

        await expect(betting.connect(owner).distributePrizes(["Random String"]))
            .to.be.revertedWith("revert You can't distribute the prizes when a game is in progress!");
    });
    it("Winner should get right amount of money.", async () => {
        const Betting = await ethers.getContractFactory("Betting");
        const betting = await Betting.deploy();
        await betting.deployed();
        const [owner, addr1, addr2] = await ethers.getSigners();

        // round it to work around the gas fee
        let winnerBalanceBefore = parseFloat(ethers.utils.formatEther(await addr1.getBalance())).toFixed(4);
        let loserBet = 2000000000000000;

        // game workflow
        await betting.connect(addr1).bet("Random String", {value: 1});
        await betting.connect(addr2).bet("Hallo", {value: loserBet});
        await betting.connect(owner).startGame();
        await betting.connect(owner).finishGame();
        await betting.connect(owner).distributePrizes(["Random String"]);

        // (balance after) - (bet of the loser) + (bet of winner) = expected balance before
        let expectedBefore = ethers.utils.formatEther(await addr1.getBalance()) - ethers.utils.formatEther(loserBet) + ethers.utils.formatEther(1);

        // rounding to fixed value because of the gas fee
        await expect(parseFloat(expectedBefore).toFixed(4)).to.equal(parseFloat(winnerBalanceBefore).toFixed(4));
    });

});