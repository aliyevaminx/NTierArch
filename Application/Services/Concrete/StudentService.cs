using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Core.Extensions;
using Data.UnitOfWork.Abstract;
using Data.UnitOfWork.Concrete;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Application.Services.Concrete;

public class StudentService : IStudentService
{
    private readonly UnitOfWork _unitOfWork;

    public StudentService()
    {
        _unitOfWork = new UnitOfWork();
    }

    public void GetAllStudents()
    {
        foreach (var student in _unitOfWork.Students.GetAll())
        {
            Console.WriteLine($"Id: {student.Id} Name: {student.Name} Surname: {student.Surname}");
        }
    }

    public void GetStudent()
    {
        GetAllStudents();

    InputIdLine: Messages.InputMessage("student Id");
        var inputId = Console.ReadLine();
        int input;
        bool isTrueIdFormat = int.TryParse(inputId, out input);
        if (!isTrueIdFormat)
        {
            Messages.InvalidInputMessage("student Id");
            goto InputIdLine;
        }

        var student = _unitOfWork.Students.GetById(input);
        if (student is null)
        {
            Messages.NotFoundMessage("student");
            return;
        }

        var group = _unitOfWork.Groups.GetById(student.GroupId);
        Console.WriteLine($"Id: {student.Id} Name: {student.Name} Surname: {student.Surname} " +
            $"Email: {student.Email} Birth Date: {student.BirthDate} Group Name: {group.Name} ");

    }

    public void AddStudent()
    {
        if (_unitOfWork.Groups.GetAll().Count() > 0)
        {
        EnterStudentNameLine: Messages.InputMessage("student name");
            string studentName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(studentName))
            {
                Messages.InvalidInputMessage("Student name");
                goto EnterStudentNameLine;
            }

        EnterStudentSurnameLine: Messages.InputMessage("student surname");
            string studentSurname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(studentSurname))
            {
                Messages.InvalidInputMessage("student surname");
                goto EnterStudentSurnameLine;
            }

        EnterStudentEmailLine: Messages.InputMessage("student email");
            string studentEmail = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(studentEmail) || !studentEmail.IsValidEmail())
            {
                Messages.InvalidInputMessage("Email");
                goto EnterStudentEmailLine;
            }

        EnterStudentBirthDate: Messages.InputMessage("birth date(dd.MM.yyyy)");
            string birthDateInput = Console.ReadLine();
            DateTime birthDate;
            bool isTrueFormat = DateTime.TryParseExact(birthDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate);

            if (DateTime.Now.Year - birthDate.Year < 6)
            {
                Messages.MustBeGivenYearsOld("6");
                return;
            }

            if (!isTrueFormat)
            {
                Messages.InvalidInputMessage("Birth date");
                goto EnterStudentBirthDate;
            }

            foreach (var group in _unitOfWork.Groups.GetAll())
            {
                Console.WriteLine($"Id: {group.Id} Name: {group.Name} Limit: {group.Limit} Begin Date: {group.BeginDate} End Date: {group.EndDate} ");
            }

        EnterGroupIdAddStudent: Messages.InputMessage("group id");
            string groupIdInput = Console.ReadLine();
            int groupId;
            isTrueFormat = int.TryParse(groupIdInput, out groupId);
            var existGroup = _unitOfWork.Groups.GetById(groupId);
            var studentsCount = _unitOfWork.Students.GetAll().Count(s => s.GroupId == groupId);

            if (studentsCount > existGroup.Limit)
            {
                Messages.HasAlreadyMessage("Group", "filled");
                return;
            }

            if (!isTrueFormat || existGroup is null)
            {
                Messages.InvalidInputMessage("group id");
                goto EnterGroupIdAddStudent;
            }


            Student student = new Student
            {
                Name = studentName,
                Surname = studentSurname,
                Email = studentEmail,
                BirthDate = birthDate,
                GroupId = groupId
            };

            _unitOfWork.Students.Add(student);
            _unitOfWork.Commit();

            Messages.SuccessMessage("Student", "added");
        }
        else
            Messages.HasNotMessage("group", "to add student");
    }

    public void UpdateStudent()
    {
        GetAllStudents();

    EnterStudentIdLine: Messages.InputMessage("student id");
        string studentIdInput = Console.ReadLine();
        int studentId;
        bool isTrueFormat = int.TryParse(studentIdInput, out studentId);

        if (!isTrueFormat)
        {
            Messages.InvalidInputMessage("Student Id");
            goto EnterStudentIdLine;
        }

        var student = _unitOfWork.Students.GetById(studentId);
        if (student is null)
        {
            Messages.NotFoundMessage("Student");
            return;
        }

    WantToChangeNameLine: Messages.WantToChangeMessage("student name");
        var choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto WantToChangeNameLine;
        }

        string newName = string.Empty;
        if (choice == "y")
        {
        NewNameInputLine: Messages.InputMessage("new name");
            newName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newName))
                goto NewNameInputLine;
        }

    WantToChangeSurnameLine: Messages.WantToChangeMessage("student surname");
        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto WantToChangeSurnameLine;
        }

        string newSurname = string.Empty;
        if (choice == "y")
        {
        NewSurnameInputLine: Messages.InputMessage("new surname");
            newSurname = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newSurname))
                goto NewSurnameInputLine;
        }

    WantToChangeEmailLine: Messages.WantToChangeMessage("student email");
        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto WantToChangeEmailLine;
        }

        string newEmail = string.Empty;
        if (choice == "y")
        {
        NewEmailInputLine: Messages.InputMessage("new surname");
            newEmail = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newSurname) || !newEmail.IsValidEmail())
                goto NewEmailInputLine;
        }

    WantToChangeBirthDateLine: Messages.WantToChangeMessage("student birth date");
        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto WantToChangeBirthDateLine;
        }

        DateTime newBirthDate = student.BirthDate;
        if (choice == "y")
        {
        NewBirthDateInputLine: Messages.InputMessage("new birth date");
            string newBirthDateInput = Console.ReadLine();
            isTrueFormat = DateTime.TryParseExact(newBirthDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newBirthDate);

            if (DateTime.Now.Year - newBirthDate.Year < 6)
            {
                Messages.MustBeGivenYearsOld("6");
                return;
            }

            if (!isTrueFormat)
            {
                Messages.InvalidInputMessage("new birth date");
                goto NewBirthDateInputLine;
            }
        }



    WantToChangeGroupIdLine: Messages.WantToChangeMessage("student group");

        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto WantToChangeGroupIdLine;
        }

        int newGroupId = student.GroupId;
        if (choice == "y")
        {
            foreach (var groupItem in _unitOfWork.Groups.GetAll())
            {
                Console.WriteLine($"Id: {groupItem.Id} Name: {groupItem.Name} Limit: {groupItem.Limit} Begin Date: {groupItem.BeginDate} End Date: {groupItem.EndDate} ");
            }

        EnterNewGroupIdLine: Messages.InputMessage("new group id");
            string newGroupIdInput = Console.ReadLine();
            isTrueFormat = int.TryParse(newGroupIdInput, out newGroupId);

            var studentsCount = _unitOfWork.Students.GetAll().Count(s => s.GroupId == newGroupId);
            var group = _unitOfWork.Groups.GetById(newGroupId);

            if (studentsCount >= group.Limit)
            {
                Messages.HasAlreadyMessage("Group", "filled");
                return;
            }

            if (!isTrueFormat || group is null)
            {
                Messages.InvalidInputMessage("group id");
                goto WantToChangeGroupIdLine;
            }
        }

        if (!string.IsNullOrEmpty(newName)) { student.Name = newName; }
        if (!string.IsNullOrEmpty(newSurname)) { student.Surname = newSurname; }
        if (!string.IsNullOrEmpty(newEmail)) { student.Email = newEmail; }
        if (newBirthDate != student.BirthDate) { student.BirthDate = newBirthDate; }
        if (newGroupId != student.GroupId) { student.GroupId = newGroupId; }

        _unitOfWork.Students.Update(student);
        _unitOfWork.Commit();

        Messages.SuccessMessage("Student", "updated");
    }

    public void DeleteStudent()
    {
        GetAllStudents();
    InputIdLine: Messages.InputMessage("student Id");
        var inputId = Console.ReadLine();
        int Id;
        bool isTrueIdFormat = int.TryParse(inputId, out Id);
        if (!isTrueIdFormat)
        {
            Messages.InvalidInputMessage("Student ID");
            goto InputIdLine;
        }

        var student = _unitOfWork.Students.GetById(Id);
        if (student is null)
        {
            Messages.NotFoundMessage("Student");
            return;
        }

        _unitOfWork.Students.Delete(student);
        _unitOfWork.Commit();

        Messages.SuccessMessage("Student", "deleted");
    }


  
}
