using System.Text;  //to use it after
using System.Xml.Linq;


public class RC4        // I defined the RC4 class
{
    // Variable definition
    private byte[] S;          // array to encrypt value
    private byte[] S_decrypt;  // we need another to decrypt
    private int i;
    private int j;


    // RC4 class
    public RC4(byte[] key)
    {
        S = new byte[256];          // We create a 256 bytes array, this array is used to store the
                                    // current state of the RC4 algorithm during the encryption process
        S_decrypt = new byte[256];  // Another array of 256 bytes, This table is used to store the original
                                    // state of the RC4 algorithm so that it can be reused during the
                                    // decryption process. This is a necessary copy.


        for (int k = 0; k <= 255; k++)   // Loop that iterates from 0 to 255, inclusive, k represents the current index.

        {

            S[k] = (byte)k;             // For each index k, the value in the array S is set to be equal at k
                                        // This means that the element at index k in the array S will have the value of k.
                                        // For example, S[0] will have the value 0, S[1] -> value 1, and so on until S[255] -> value 255.
            S_decrypt[k] = (byte)k;     // Idem but for S_decrypt

        }

        int keyLength = key.Length;     // Get the key length (number of bytes)
        j = 0;

        // We use k to shuffle the array
        for (int k = 0; k < 256; k++)
        {
            j = (j + S[k] + key[k % keyLength]) % 256;  // Update the index j using S and the key
            Swap(S, k, j);                              // Swap positions of elements at indices k and j in array S
        }

        // Save the original state for decryption
        Array.Copy(S, S_decrypt, 256);
        i = j = 0;
    }


    // Encryption method 
    public byte[] Encrypt(byte[] input)
    {
        byte[] output = new byte[input.Length];      // Array to store the encrypted output
        for (int k = 0; k < input.Length; k++)
        {
            i = (i + 1) % 256;                       //update i
            j = (j + S[i]) % 256;                    //update j using S
            Swap(S, i, j);                           // Swap elements at indices i and j in array S

            output[k] = (byte)(input[k] ^ S[(S[i] + S[j]) % 256]);  // XOR operation for encryption
        }
        return output;
    }


    // Decryption method
    public byte[] Decrypt(byte[] input)
    {
        byte[] output = new byte[input.Length];     // Array to store the decrypted output
        i = 0; j = 0;
        for (int k = 0; k < input.Length; k++)
        {
            i = (i + 1) % 256;                      // Update i 
            j = (j + S_decrypt[i]) % 256;           // Update j using array S_decrypt
            Swap(S_decrypt, i, j);                  // Swap elements at indices i and j in array S_decrypt

            output[k] = (byte)(input[k] ^ S_decrypt[(S_decrypt[i] + S_decrypt[j]) % 256]);   // XOR operation for dencryption
        }
        return output;
    }

    // Method to swap elements in an array to simplify the code
    private void Swap(byte[] array, int i, int j)
    {
        byte temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}
// End of RC4 class



class Program
{
    static void Main(string[] args)
    {

        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine("\n\n\t\t" +
            "░░░██║░░░██║░███████║░██║░░░░░░░██║░░░░░░░░░████║░░░░░ \n\t\t" +
            "░░░██║░░░██║░██║░░░░░░██║░░░░░░░██║░░░░░░░██║░░░██║░░░ \n\t\t" +
            "░░░██║░░░██║░██║░░░░░░██║░░░░░░░██║░░░░░░░██║░░░██║░░░ \n\t\t" +
            "░░░████████║░█████║░░░██║░░░░░░░██║░░░░░░░██║░░░██║░░░ \n\t\t" +
            "░░░██║░░░██║░██║░░░░░░██║░░░░░░░██║░░░░░░░██║░░░██║░░░ \n\t\t" +
            "░░░██║░░░██║░██║░░░░░░██║░░░░░░░██║░░░░░░░██║░░░██║░░░ \n\t\t" +
            "░░░██║░░░██║░███████║░████████║░████████║░░░████║░░░░░ \n\n");

        Console.WriteLine("\n\t\t                 Let's Encrypt Data   \n\n");

        Console.ResetColor();  // To have original color
        Console.ReadLine();  // Wait for an enter of the user

        Console.Write("Enter a secret key in binary form ! (be aware of person who are around you) : ");
        string keyInput = Console.ReadLine();

        // Convert the binary string to a byte array
        byte[] key = new byte[keyInput.Length];
        for (int i = 0; i < keyInput.Length; i++)
        {
            if (keyInput[i] == '1')
            {
                key[i] = 1;
            }
            else
            {
                key[i] = 0;
            }
        }

        Console.Write("Enter the message to encrypt : ");
        string message = Console.ReadLine();

        RC4 rc4 = new RC4(key);             // Create an instance of the RC4 class with the specified key
        byte[] encrypted = rc4.Encrypt(Encoding.UTF8.GetBytes(message)); // Encrypt the UTF-8 encoded message using the RC4 instance
        byte[] decrypted = rc4.Decrypt(encrypted);                       // Decrypt it using the same method

        Console.Write("Encrypted message : ");
        foreach (byte b in encrypted)
        {
            string hexValue = b.ToString("X2");  // Convert the byte to a hexadecimal string with two digits
            Console.Write(hexValue + "-");
        }
        Console.WriteLine();

        Console.WriteLine($"Decrypted Message : {Encoding.UTF8.GetString(decrypted)}");
    }
}
