public class Station
{
    TestedPlayer owner; //所有者, player1 = 1,player2 = 2, ...
    int level;//等级
    int def;//防御
    int atk;//攻击
    int maxHp;//血量最大值
    int hp;//血量
    int energyPerTurn;//每回合产生能源

    public Station(TestedPlayer ow)
    {
        owner = ow;
        level = 0;
        atk = 0;
        def = 2;
        maxHp = 6;
        hp = 6;
        energyPerTurn = 2;
    }

    public bool isOwner(TestedPlayer tp)
    {
        return owner == tp;
    }
    public void gainEnergy()
    {
        owner.setEnergy(energyPerTurn);
    }
}
