using System;
Console.Title = "Wizards & Warriors";

//Here be variables yarr
string boxLength = "-----------------------";
string[] names = { null, "Barbarian" }, attackList = { "Frozen", "Attack", "PowerAttack", "Fireblast", "Frostbolt", "Attempt Heal", "Block" };
int[] hitPoints = { 20, 20, 0, 0 }, attackSelection = { 0, 0 }, healAmount = { 0, 0, 1, 1, 2, 3, 2, 1, 1, 0, 0, 0 };
int activePlayer = 0, passivePlayer = 1, turnCounter = 1, playerMode;
bool[] isFrozen = { false, false }, onFire = { false, false };
//Welcome Message
Console.WriteLine("------------------------Welcome to Wizards & Warriors------------------------");
Console.Write("Choose GameMode:\n1: Singleplayer \n2: Multiplayer \n"); playerMode = int.Parse(Console.ReadLine());
Console.Write("ENTER NAME PLAYER 1: "); names[0] = Console.ReadLine();
if (playerMode == 2) { Console.Write("ENTER NAME PLAYER 2: "); names[1] = Console.ReadLine(); }
while (true) //GameLoop
{
    Console.Clear();   //UserInterfaceArea
    Console.WriteLine($"------------------------Fight Turn: {turnCounter}------------------------");
    Console.WriteLine($"{names[0]} (HP): {hitPoints[0],-20}{names[1],20} (HP): {hitPoints[1]} \n\n\n\n");
    if (turnCounter > 2) Console.WriteLine($"{names[0]}s last move was {attackList[attackSelection[0]],-20} {names[1]}s last move was {attackList[attackSelection[1]]}"); // Prints earlier actions

    Console.WriteLine(boxLength);  //AttackSelectionBox
    for (int i = 1; i < attackList.Length; i++)
    {
        Console.WriteLine($"|{i}. {attackList[i],-18}|");
    }
    Console.WriteLine(boxLength);

    if (activePlayer == 0 || playerMode == 2) //Commandinput for player and also player 2 if two-player mode. 
    {
        Console.WriteLine($"{names[activePlayer]} choose your move: ");
        attackSelection[activePlayer] = int.Parse(Console.ReadLine());
    }
    else //Command input for computer enemy in single player mode
    {
        Console.WriteLine($"{names[1]} is attacking");
        attackSelection[1] = new Random().Next(1, attackList.Length);
        System.Threading.Thread.Sleep(1500);
    }

    if (isFrozen[activePlayer] == true) { attackSelection[activePlayer] = 0; isFrozen[activePlayer] = false; } //Cant attack if you are frozen bruh. Also removes frozen effect.

    switch (attackSelection[activePlayer]) //AttackEffects
    {
        case 1: hitPoints[passivePlayer]--; break;                                                                          //NormalAttack - 1DMG
        case 2: hitPoints[passivePlayer] -= new Random().Next(0, 4); break;                                                 //PowerAttack - 0-3 DMG
        case 3: onFire[passivePlayer] = true; hitPoints[passivePlayer]--; break;                                            //FireBlast - 1DMG + 1DMG next round
        case 4: if (new Random().Next(0, 3) == 0) { isFrozen[passivePlayer] = true; hitPoints[passivePlayer]--; } break;    //FrostBolt - 33% Chance of doing 1 dmg and freezing enemy for the next round
        case 5: hitPoints[activePlayer] += healAmount[new Random().Next(0, healAmount.Length)]; break;                      //Attempt Heal - Heals 0-3 HP
        case 6: hitPoints[activePlayer + 2] = hitPoints[activePlayer]; break;                                               //Block - Blocks next incoming attack but wont stop status effects
        default: break;
    }

    if (attackSelection[passivePlayer] == 6) hitPoints[passivePlayer] = hitPoints[passivePlayer + 2]; //Blocks attack by restoring HP to earlier setpoint. This means that you can block the attack but still be on fire or frozen. 

    if (onFire[activePlayer] == true) { hitPoints[activePlayer]--; if (new Random().Next(0, 2) == 0) onFire[activePlayer] = false; } //Apply Fire Damage and 50% chance to stop burning.

    if (hitPoints[passivePlayer] <= 0)  //GameEndConditions
    {
        if (passivePlayer == 0)
        {
            Console.WriteLine($"{names[passivePlayer]} were slain"); Console.ReadKey(true); break;
        }
        Console.WriteLine("Congratulations, you won the fight!"); Console.ReadKey(true); break;
    }
    activePlayer = activePlayer == 0 ? activePlayer = 1 : activePlayer = 0;  //Switches the activeplayer to the passive player to reverse the next round
    passivePlayer = passivePlayer == 1 ? passivePlayer = 0 : passivePlayer = 1;
    turnCounter++;
}