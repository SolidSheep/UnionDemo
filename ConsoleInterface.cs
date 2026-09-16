using static UnionExample.UserService;

namespace UnionExample
{
    public sealed class ConsoleInterface
    {
        private static UserService _service = UserService.Instance;

        public static void LoadUsers()
        {
            while (true)
            {
                try
                {
                    _service.Initialise();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(" Exception triggered in LoadUsers. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        public static void LookupUsers()
        {
            while (true)
            {
                Console.Write("Enter a user's name to retrieve user ID. A new name will add a new user. Or type ''exit'': ");
                string? name = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(name)) continue;
                if (!CheckIfNameIsValid(name)) continue;
                name = name.ToLowerInvariant();
                if (name.Equals("exit")) { return; }

                // UserResult is a union, you can change the types in UserResult.cs and then handle them in DisplayResult()
                UserResult result = _service.GetOrAddUser(name);
                DisplayResult(result);
            }
        }

        private static bool CheckIfNameIsValid(string name)
        {
            bool result = true;
            int minLength = 2;
            if (name.Count(char.IsLetter) < minLength)
            {
                Console.WriteLine("Name must be at least 2 characters.");
                result = false;
            }
            if (!name.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
            {
                Console.WriteLine("Name must only contain letters, spaces or hyphens.");
                result = false;
            }
            return result;
        }

        static void DisplayResult(UserResult result)
        {
            switch (result)
            {
                case float id:
                    Console.WriteLine($"ID: {id}");
                    break;

                case string message:
                    Console.WriteLine(message);
                    break;

                case Exception ex:
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
            }
        }
    }
}
