using Application.Services.Concrete;
using Core.Constants;

namespace NTierArch;

public static class Program
{

    private static readonly GroupService _groupService;
    private static readonly StudentService _studentService;

    static Program()
    {
        _groupService = new GroupService();
        _studentService = new StudentService();
    }

    public static void Main(string[] args)
    {
        while (true)
        {
            ShowMenu();

            string optionInput = Console.ReadLine();
            int option;
            bool isTrueFormat = int.TryParse(optionInput, out option);
            if (isTrueFormat)
            {
                switch ((Options)option)
                {
                    case Options.GetAllGroups:
                        _groupService.GetAllGroups();
                        break;
                    case Options.GetGroup:
                        _groupService.GetGroup();
                        break;
                    case Options.AddGroup:
                        _groupService.AddGroup();
                        break;
                    case Options.UpdateGroup:
                        _groupService.UpdateGroup();
                        break;
                    case Options.DeleteGroup:
                        _groupService.DeleteGroup();
                        break;
                    case Options.GetAllStudents:
                        _studentService.GetAllStudents();
                        break;
                    case Options.GetStudent:
                        _studentService.GetStudent();
                        break;
                    case Options.AddStudent:
                        _studentService.AddStudent();
                        break;
                    case Options.UpdateStudent:
                        _studentService.UpdateStudent();
                        break;
                    case Options.DeleteStudent:
                        _studentService.DeleteStudent();
                        break;
                    case Options.Exit:
                        return;
                    default:
                        Messages.InvalidInputMessage("Choice");
                        break;
                }
            }

        }
    }

    public static void ShowMenu()
    {
        Console.WriteLine("0. Exit");
        Console.WriteLine("1. All Groups");
        Console.WriteLine("2. Details of Group");
        Console.WriteLine("3. Add Group");
        Console.WriteLine("4. Update Group");
        Console.WriteLine("5. Delete Group");
        Console.WriteLine("6. All Students");
        Console.WriteLine("7. Details of Student");
        Console.WriteLine("8. Add Student");
        Console.WriteLine("9. Update Student");
        Console.WriteLine("10. Delete of Student");
    }
}
