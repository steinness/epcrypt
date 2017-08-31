using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FIX UNICODE

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
			string unicodeCheck;
			string text;
			string password;
			bool unicode = false;

			// Set encoding to unicode
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			while (true) {
				Console.Write("Encrypt or decrypt? (d/e): ");
				encryptOrDecrypt = Console.ReadLine();
				Console.Write("Use unicode encryption? (y/n): ");
				unicodeCheck = Console.ReadLine();
				unicode = true;

				if (unicodeCheck == "N" || unicodeCheck == "n") {
					unicode = false;
				}
				

				if (encryptOrDecrypt == "E" || encryptOrDecrypt == "e") {
					Console.Write("What do you want to encrypt?: ");
					text = Console.ReadLine();
					Console.Write("What is your password?: ");

					password = Console.ReadLine();
					Console.WriteLine(Epcrypt.Encrypt(password, text, unicode));
					Console.WriteLine("\n");

				}
				else if (encryptOrDecrypt == "D" || encryptOrDecrypt == "d") {
					Console.Write("What do you want to decrypt?: ");
					text = Console.ReadLine();
					Console.Write("What is your password?: ");

					password = Console.ReadLine();
					Console.WriteLine(Epcrypt.Decrypt(password, text, unicode));
					Console.WriteLine("\n");
				}
			}
		}
	}
}
