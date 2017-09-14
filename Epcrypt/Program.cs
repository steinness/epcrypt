using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// FIX UNICODE
// Git test :D

namespace Epcrypt {
	public class Epcrypt {

		// All the alphabets used for encryption
		static string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.,!?-;:/(){}[]=*\"\\#0123456789 ";
		static string customUnicodeAlphabet = "¥¢£¤%@!¡å$±ł®ª©↓→←πæßðđŋ€øʘ♠♦♣♥♪♫☺☻☯⚘☮⚰⚱♡♢♤♧♬⌘▹◁▷◅▻◬⊿∆∇◭◮◉⬔▨₡ḠḴḀḚṖṀṘ₷Ḩℇ₩฿₹™μψƔꗦꕬ";
		static string customASCIIAlphabet = "mMnNbBvVcCxXzZaAsSdDfFgGhHjJkKlLpPoOiIuUyYtTrReEwWqQ1234567890+-.,\\/#;:()}{%&<>|?!";

		static int amountOfCharactersInAlphabet = 82;

		// For encrypting one character
		private static string Echar(int shift, string character, bool unicode) {
			int characterPosition;
			characterPosition = alphabet.IndexOf(character);

			int newCharacterPosition = characterPosition + shift;

			// The shift number has to be smaller than the amount of characters in the custom alphabet
			while (newCharacterPosition >= amountOfCharactersInAlphabet) {
				newCharacterPosition -= amountOfCharactersInAlphabet;
			}

			if (unicode == true) {
				// Converts string to char because Replace() only takes char as the first argument
				character = character.Replace((Convert.ToChar(character)), customUnicodeAlphabet[newCharacterPosition]);
			}
			else if (unicode == false) {
				// Converts string to char because Replace() only takes char as the first argument
				character = character.Replace((Convert.ToChar(character)), customASCIIAlphabet[newCharacterPosition]);
			}
			return character;
		}

		// For decrypting one character
		private static string Dchar(int shift, string character, bool unicode) {
			int characterPosition;
			characterPosition = 0;

			if (unicode == true) {
				characterPosition = customUnicodeAlphabet.IndexOf(character);
			}
			else if (unicode == false) {
				characterPosition = customASCIIAlphabet.IndexOf(character);
			}

			int newCharacterPosition = characterPosition - shift;

			// The shift number have to be between 1 and the amount of characters in the custom alphabet for the algorithm to work
			while (newCharacterPosition <= 0) {
				newCharacterPosition += amountOfCharactersInAlphabet;
			}
			while (newCharacterPosition >= amountOfCharactersInAlphabet) {
				newCharacterPosition -= amountOfCharactersInAlphabet;
			}

			character = character.Replace((Convert.ToChar(character)), alphabet[newCharacterPosition]);
			return character;
		}

		// For encrypting a whole string
		public static string Encrypt(string password, string text, bool unicode) {
			string encryptedText = "";
			int shift = ShiftGenerator(password);

			foreach (char character in text) {
				encryptedText += Echar(shift, Convert.ToString(character), unicode);
			}
			return encryptedText;
		}

		// For decrypting a whole string
		public static string Decrypt(string password, string text, bool unicode) {
			string decryptedText = "";
			int shift = ShiftGenerator(password);

			foreach (char character in text) {
				decryptedText += Dchar(shift, Convert.ToString(character), unicode);
			}
			return decryptedText;
		}

		// For encrypting files
		public static void EncryptFile(string password, string filepath, bool unicode) {
			string newFilepath = filepath.Insert((filepath.IndexOf('.')), "_e");

			foreach (string line in File.ReadLines(filepath)) {
				string encryptedLine = Encrypt(password, line, unicode);
				File.AppendAllText(newFilepath, encryptedLine);
				File.AppendAllText(newFilepath, "\n\r");
			}
		}

		// For decrypting files
		public static void DecryptFile(string password, string filepath, bool unicode) {
			string newFilepath = filepath.Insert((filepath.IndexOf('.')), "_d");

			foreach (string line in File.ReadLines(filepath)) {
				string decryptedLine = Decrypt(password, line, unicode);
				File.AppendAllText(newFilepath, decryptedLine);
				File.AppendAllText(newFilepath, "\n\r");
			}
		}

		// For generating a shift number from string
		private static int ShiftGenerator(string password) {
			int s = 0;

			foreach (char character in password) {
				s += (alphabet.IndexOf(character));
			}

			return s;
		}

	}

	class Program {
		static void Main(string[] args) {
			string encryptOrDecrypt;
			//string unicodeCheck;
			string fileOrNot;
			string text;
			string password;
			string filepath;
			bool unicode = false;

			// Set encoding to unicode (doesn't work :/)
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			while (true) {
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("Encrypt or decrypt? (d/e): ");
				encryptOrDecrypt = Console.ReadLine();
				if (encryptOrDecrypt == "e" || encryptOrDecrypt == "E") {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("Encrypt a file? (y/n): ");
					fileOrNot = Console.ReadLine();
				}
				else {
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Write("Decrypt a file? (y/n): ");
					fileOrNot = Console.ReadLine();
				}
				unicode = false;
				/*
				Console.Write("Use unicode encryption? (y/n): ");
				unicodeCheck = Console.ReadLine();
				

				if (unicodeCheck == "N" || unicodeCheck == "n") {
					unicode = false;
				}
				*/
				
				if (encryptOrDecrypt == "E" || encryptOrDecrypt == "e") {
					if (fileOrNot == "y" || fileOrNot == "Y") {
						Console.Write("Filepath: ");
						filepath = Console.ReadLine();
						Console.Write("What is your password? ");
						password = Console.ReadLine();
						Console.ForegroundColor = ConsoleColor.White;
						Epcrypt.EncryptFile(password, filepath, unicode);
						Console.WriteLine("Done!");
					}
					else {

						Console.Write("What do you want to encrypt?: ");
						text = Console.ReadLine();
						Console.Write("What is your password?: ");

						password = Console.ReadLine();
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(Epcrypt.Encrypt(password, text, unicode));
						Console.WriteLine("\n");
					}
				}
				else if (encryptOrDecrypt == "D" || encryptOrDecrypt == "d") {
					if (fileOrNot == "y" || fileOrNot == "Y") {
						Console.Write("Filepath: ");
						filepath = Console.ReadLine();
						Console.Write("What is your password? ");
						password = Console.ReadLine();
						Console.ForegroundColor = ConsoleColor.White;
						Epcrypt.DecryptFile(password, filepath, unicode);
						Console.WriteLine("Done!");
					}
					else {
						Console.Write("What do you want to decrypt?: ");
						text = Console.ReadLine();
						Console.Write("What is your password?: ");

						password = Console.ReadLine();
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine(Epcrypt.Decrypt(password, text, unicode));
						Console.WriteLine("\n");
					}
				}
			}
		}
	}
}
