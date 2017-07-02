import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;
import javax.xml.bind.DatatypeConverter;
import java.nio.charset.Charset;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.List;
import java.math.BigInteger;

/**
 * Created by Krompy on 7/1/2017.
 */

public class Main
{
    private static List<String> items = new ArrayList<>();
    private static int count;
    private static String key;
    private static int keyLength = 0;

    public static void main(String[] args)
    {
        GetRules(args);
        GetKeyLength();
        PrintHelp();
        LetsPlay();
    }

    private static void LetsPlay()
    {
        String str = "";
        int computer = 0;
        while (!str.equalsIgnoreCase("quit"))
        {
            if (!str.isEmpty())
            {
                if (str.equalsIgnoreCase("help"))
                    PrintHelp();
                else {
                    int player = GetPlayersInput(str) - 1;
                    while (!isIndexExists(player)) {
                        System.out.print("Doesn't exist. Please, try again:\n");
                        str = System.console().readLine();
                        while (str.equalsIgnoreCase("quit") || str.equalsIgnoreCase("help")) {
                            if (str.equalsIgnoreCase("quit")) return;
                            if (str.equalsIgnoreCase("help")) {
                                PrintHelp();
                                str = System.console().readLine();
                            }
                        }
                        player = GetPlayersInput(str);
                    }
                    int winner = isPlayerWins(computer, player);
                    PrintWinner(winner, computer, player);
                }
            }
            computer = ComputersChoise();
            SetProof(items.get(computer));
            str = System.console().readLine();
        }
    }

    private static void GetKeyLength()
    {
        for (String item: items) if (keyLength < item.length()) keyLength = item.length();
    }

    private static void PrintWinner(int winner, int computer, int player)
    {
        if (winner == 1)
        {
            GetProof();
            System.console().printf("You're win!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
        else if (winner == 0)
        {
            GetProof();
            System.console().printf("Oups, same chiose!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
        else
        {
            GetProof();
            System.console().printf("Computer wins!\nComputer: %s\nYou: %s\n\n", items.get(computer), items.get(player));
        }
    }

    private static void SetProof(String item)
    {
        SecureRandom random = new SecureRandom();
        key = new BigInteger(keyLength * 8, random).toString(32);
        String proof = "";
        try { proof = GenerateHash(key, item); } catch (NoSuchAlgorithmException e) { }
        System.console().printf("Computers encoded choise: %s\n", proof);
    }

    public static String GenerateHash(String key, String item) throws NoSuchAlgorithmException
    {
        Charset utfCs = Charset.forName("US-ASCII");
        Mac sha256 = Mac.getInstance("HmacSHA256");
        SecretKeySpec secretKey = new SecretKeySpec(utfCs.encode(key).array(), "HmacSHA256");
        try { sha256.init(secretKey); }
        catch (InvalidKeyException e) { e.printStackTrace(); }
        byte[] mac_data = sha256.doFinal(utfCs.encode(item).array());
        String result = DatatypeConverter.printHexBinary(mac_data);
        return result;
    }

    private static void GetProof()
    {
        System.console().printf("Key: %s\n", key);
    }

    private static int GetPlayersInput(String str)
    {
        try { return Integer.parseInt(str); }
        catch (Exception e)
        {
            System.out.print("Please, try again:\n");
            str = System.console().readLine();
            if (str.equalsIgnoreCase("help"))
                while (str.equalsIgnoreCase("help"))
                {
                    PrintHelp();
                    str = System.console().readLine();
                }
            else return GetPlayersInput(str);
            return GetPlayersInput(System.console().readLine());
        }
    }

    private static int isPlayerWins(int computer, int player)
    {
        if (player < computer)
            return ((computer + (player % 2)) % 2 == 1) ? 1 : -1;
        if (player > computer)
            return ((computer + (player % 2)) % 2 == 0) ? 1 : -1;
        return 0;
    }

    private static boolean isIndexExists(int i) { return (i < items.size() && i >= 0); }

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
