import com.sun.deploy.util.SyncAccess;

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
        while (!System.console().readLine().equalsIgnoreCase("quit"))
        {
            // some game logic
            System.out.print("lol\n");
        }
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
