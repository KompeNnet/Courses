import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Krompy on 7/1/2017.
 */

public class Main
{
    private static List<String> items = new ArrayList<>();
    private static int count;

    public static void main(String[] args)
    {
        GetRules(args);
        PrintHelp();
        LetsPlay();
    }

    private static void LetsPlay()
    {
        String str = System.console().readLine();
        while (!str.equalsIgnoreCase("quit"))
        {
            int computer = ComputersChoise();
            GetProof();
            if (str.equalsIgnoreCase("help"))
                PrintHelp();
            else
            {
                int player = GetPlayersInput(str) - 1;
                while (!isIndexExists(player))
                {
                    System.out.print("Doesn't exist. Please, try again:\n");
                    PrintHelp();
                    str = System.console().readLine();
                    if (str.equalsIgnoreCase("quit")) return;
                    else if (str.equalsIgnoreCase( "help")) PrintHelp();
                    else player = GetPlayersInput(str);
                }
                int winner = isPlayerWin(computer, player);
                PrintWinner(winner, computer, player);
            }
            str = System.console().readLine();
        }
    }

    private static void PrintWinner(int winner, int computer, int player)
    {
        if (winner == 1)
        {
            PrintProof();
            System.console().printf("You're win!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
        else if (winner == 0)
        {
            PrintProof();
            System.console().printf("Oups, same chiose!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
        else
        {
            PrintProof();
            System.console().printf("Computer wins!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
    }

    private static void GetProof()
    {
        // proof here
    }

    private static void PrintProof()
    {
        // proof print here
    }

    private static int GetPlayersInput(String str)
    {
        try { return Integer.parseInt(str); }
        catch (Exception e)
        {
            System.out.print("Please, try again:\n");
            PrintHelp();
            return GetPlayersInput(System.console().readLine());
        }
    }

    private static int isPlayerWin(int computer, int player)
    {
        if (player < computer)
            return ((computer + (player % 2)) % 2 == 1) ? 1 : -1;
        if (player > computer)
            return ((computer + (player % 2)) % 2 == 0) ? 1 : -1;
        return 0;
    }

    private static boolean isIndexExists(int i)
    {
        return (i < items.size() && i >= 0);
    }

    private static int ComputersChoise()
    {
        SecureRandom random = new SecureRandom();
        return random.nextInt(count);
    }

    private static void PrintHelp()
    {
        int i = 1;
        for (String item: items)
        {
            System.out.format("%d. %s\n", i, item);
            i++;
        }
    }

    private static void GetRules(String[] args)
    {
        try { count = (Integer.parseInt(args[0]) - 1) / 2 * 2 + 1; }
        catch (Exception e)
        {
            GetDefaultRules();
            return;
        }
        if (count < 3)
        {
            GetDefaultRules();
            return;
        }
        System.out.print("Put rule elements:\n");
        for (int i = 0; i < count; i++)
            items.add(GetNewItem());
    }

    private static String GetNewItem()
    {
        String item = System.console().readLine();
        if (item.isEmpty() || items.contains(item))
        {
            System.out.print("Try again:\n");
            item = GetNewItem();
        }
        return item;
    }

    private static void GetDefaultRules()
    {
        count = 3;
        items.add("rock");
        items.add("scissors");
        items.add("paper");
    }
}
