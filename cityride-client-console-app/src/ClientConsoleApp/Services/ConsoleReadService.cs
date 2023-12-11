using CityRide.Domain.Enums;

using ClientConsoleApp.Services.Interfaces;

namespace ClientConsoleApp.Services;

public class ConsoleReadService : IConsoleReadService
{
    private readonly string _helpOption;

    public ConsoleReadService(string helpOption)
    {
        _helpOption = helpOption;
    }
    
    public double? ReadDouble()
    {
        string? input = Console.ReadLine();

        try
        {
            double value = Convert.ToDouble(input);
            return value;
        }
        catch (FormatException formatException)
        {
            Console.WriteLine("Your input was in incorrect format, please try again.");
            // TODO: log exception message
        }
        catch (OverflowException overflowException)
        {
            Console.WriteLine("Your input value was too large, please try again.");
            // TODO: log exception message
        }
        catch (ArgumentNullException argumentNullException)
        {
            Console.WriteLine("No input was provided, please try again");
            // TODO: log exception message
        }
        catch (Exception exception)
        {
            Console.WriteLine("Something went wrong, please try again.");
            // TODO: log exception message
        }

        return null;
    }

    public CarClass? ReadCarClass()
    {
        Console.Write($"Please enter desired car class (enter \"{_helpOption}\" to view options): ");
        
        string? input = Console.ReadLine();

        if (input == _helpOption)
        {
            // Print available car classes
            Console.WriteLine("Available car classes:");
            foreach (var carClassOption in Enum.GetValues(typeof(CarClass)))
            {
                Console.WriteLine($"For {carClassOption} - input {(int)carClassOption}");
            }

            return null;
        }

        try
        {
            int carClassAsInteger = Convert.ToInt32(input);
            if (!Enum.IsDefined(typeof(CarClass), carClassAsInteger))
            {
                throw new ArgumentOutOfRangeException();
            }

            CarClass carClass = (CarClass)carClassAsInteger;
            return carClass;
        }
        catch (FormatException formatException)
        {
            Console.WriteLine("Your input was in incorrect format, please try again.");
            // TODO: log exception message
        }
        catch (OverflowException overflowException)
        {
            Console.WriteLine("Your input value was too large, please try again.");
            // TODO: log exception message
        }
        catch (ArgumentOutOfRangeException argumentOutOfRangeException)
        {
            Console.WriteLine("Your input value was not one of the allowed options, please try again.");
            // TODO: log exception message
        }
        catch (ArgumentNullException argumentNullException)
        {
            Console.WriteLine("No input was provided, please try again");
            // TODO: log exception message
        }
        catch (Exception exception)
        {
            Console.WriteLine("Something went wrong, please try again.");
            // TODO: log exception message
        }

        return null;
    }
}