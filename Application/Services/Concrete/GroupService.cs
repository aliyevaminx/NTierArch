using Application.Services.Abstract;
using Core.Entities;
using Data.UnitOfWork.Concrete;
using System.Globalization;
using Core.Constants;
using Core.Extensions;

namespace Application.Services.Concrete;

public class GroupService : IGroupService
{
    private  readonly UnitOfWork _unitOfWork;

    public GroupService()
    {
        _unitOfWork = new UnitOfWork();
    }

    public  void GetAllGroups()
    {
        _unitOfWork.Groups.GetAll().Count();
        if (_unitOfWork.Groups.GetAll().Count() <= 0)
            Messages.NotFoundMessage("Groups");

        foreach (var group in _unitOfWork.Groups.GetAll())
        {
            Console.WriteLine($"Id: {group.Id} Name: {group.Name} Limit: {group.Limit} Begin Date: {group.BeginDate} End Date: {group.EndDate} ");
        }
    }

    public  void GetGroup()
    {
        GetAllGroups();

    InputIdLine: Messages.InputMessage("group Id");
        var inputId = Console.ReadLine();
        int input;
        bool isTrueIdFormat = int.TryParse(inputId, out input);
        if (!isTrueIdFormat)
        {
            Messages.InvalidInputMessage("group Id");
            goto InputIdLine;
        }

        var group = _unitOfWork.Groups.GetById(input);
        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            return;
        }

        Console.WriteLine($"Id: {group.Id} Name: {group.Name} Limit: {group.Limit} " +
            $"Begin Date: {group.BeginDate} End Date: {group.EndDate} ");
    }

    public  void AddGroup()
    {
    EnterGroupNameLine: Messages.InputMessage("group name");
        string groupName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(groupName))
        {
            Messages.InvalidInputMessage("Group name");
            goto EnterGroupNameLine;
        }

        var existedGroup = _unitOfWork.Groups.GetByName(groupName);
        if (existedGroup is not null)
        {
            Messages.AlreadyExistMessage(groupName);
            goto EnterGroupNameLine;
        }

    EnterGroupLimitLine: Messages.InputMessage("group limit");
        string limitInput = Console.ReadLine();
        int limit;
        bool isTrueFormat = int.TryParse(limitInput, out limit);
        if (!isTrueFormat || limit <= 0)
        {
            Messages.InvalidInputMessage("Group Limit");
            goto EnterGroupLimitLine;
        }


    EnterGroupBeginDate: Messages.InputMessage("begin date (dd.MM.yyyy)");
        string beginDateInput = Console.ReadLine();
        DateTime beginDate;
        isTrueFormat = DateTime.TryParseExact(beginDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out beginDate);
        if (!isTrueFormat)
        {
            Messages.InvalidInputMessage("Begin Date");
            goto EnterGroupBeginDate;
        }

    EnterGroupEndDate: Messages.InputMessage("end date (dd.MM.yyyy)");
        string endDateInput = Console.ReadLine();
        DateTime endDate;
        isTrueFormat = DateTime.TryParseExact(endDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
        if (!isTrueFormat || beginDate.Date.AddMonths(6).Date > endDate.Date)
        {
            Messages.InvalidInputMessage("End Date");
            goto EnterGroupEndDate;
        }

        Group group = new Group
        {
            Name = groupName,
            Limit = limit,
            BeginDate = beginDate,
            EndDate = endDate
        };

        _unitOfWork.Groups.Add(group);
        _unitOfWork.Commit();

        Messages.SuccessMessage("Group", "added");
    }

    public  void UpdateGroup()
    {
        if (_unitOfWork.Groups.GetAll().Count() <= 0)
        {
            Messages.HasNotMessage("any group", "to show");
            return;
        }

        GetAllGroups();
    EnterGroupIdLine: Messages.InputMessage("group Id to update");
        string groupIdInput = Console.ReadLine();
        int groupId;
        bool isTrueFormat = int.TryParse(groupIdInput, out groupId);
        var existGroup = _unitOfWork.Groups.GetById(groupId);
        if (!isTrueFormat || existGroup is null)
        {
            Messages.InvalidInputMessage("Group id");
            goto EnterGroupIdLine;
        }

    EnterChoiceForGroupName: Messages.WantToChangeMessage("group name");
        var choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("choice");
            goto EnterChoiceForGroupName;
        }

        string newGroupName = string.Empty;
        if (choice == "y")
        {
        EnterNewGroupNameLine: Messages.InputMessage("new group name");
            newGroupName = Console.ReadLine();

            var existGroupName = _unitOfWork.Groups.GetByName(newGroupName);
            if (string.IsNullOrWhiteSpace(newGroupName) || existGroupName is not null)
            {
                Messages.InvalidInputMessage("new group name");
                goto EnterNewGroupNameLine;
            }
        }

    EnterChoiceForGroupLimit: Messages.WantToChangeMessage("group limit");
        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto EnterChoiceForGroupLimit;
        }
        int newGroupLimit = existGroup.Limit;
        if (choice == "y")
        {
        EnterNewGroupLimitLine: Messages.InputMessage("new group limit");
            string newGroupLimitInput = Console.ReadLine();
            isTrueFormat = int.TryParse(newGroupLimitInput, out newGroupLimit);

            int countOfStudents = _unitOfWork.Students.GetAll().Count(s => s.GroupId == groupId); 

            if (countOfStudents > newGroupLimit)
            {
                Messages.InputMessage("correct new limit or remove some students from group.");
                return;
            }

            if (!isTrueFormat)
            {
                Messages.InvalidInputMessage("new group limit");
                goto EnterNewGroupLimitLine;
            }
        }

    EnterChoiceForGroupBeginDate: Messages.WantToChangeMessage("group begin date");
        choice = Console.ReadLine();
        if (!choice.IsValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto EnterChoiceForGroupBeginDate;
        }

        bool endDateChanged = false;
        DateTime newGroupBeginDate = existGroup.BeginDate;
        DateTime newGroupEndDate = existGroup.EndDate;
        if (choice == "y")
        {
        EnterNewGroupBeginDate: Messages.InputMessage("new group begin date");
            string newGroupBeginDateInput = Console.ReadLine();

            isTrueFormat = DateTime.TryParseExact(newGroupBeginDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newGroupBeginDate);
            if (!isTrueFormat)
            {
                Messages.InvalidInputMessage("new group begin date");
                goto EnterNewGroupBeginDate;
            }

            if (newGroupBeginDate.Date.AddMonths(6) > newGroupEndDate.Date)
            {
                Console.WriteLine("End Date must be at least 6 months later. Change end date");
            EnterNewGroupEndDate: Messages.InputMessage("new group end date");
                string newGroupEndDateInput = Console.ReadLine();

                isTrueFormat = DateTime.TryParseExact(newGroupEndDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newGroupEndDate);
                if (!isTrueFormat)
                {
                    Messages.InvalidInputMessage("new group end date");
                    goto EnterNewGroupEndDate;
                }

                if (newGroupBeginDate.Date.AddMonths(6) > newGroupEndDate.Date)
                {
                    Messages.InvalidInputMessage("End Date");
                    goto EnterNewGroupEndDate;
                }
                endDateChanged = true;
            }
        }
        if (!endDateChanged)
        {
        EnterChoiceForGroupEndDate: Messages.WantToChangeMessage("group end date");
            choice = Console.ReadLine();
            if (!choice.IsValidChoice())
            {
                Messages.InvalidInputMessage("Choice");
                goto EnterChoiceForGroupEndDate;
            }

            if (choice == "y")
            {
            EnterNewGroupEndDate: Messages.InputMessage("new group end date");
                string newGroupEndDateInput = Console.ReadLine();


                isTrueFormat = DateTime.TryParseExact(newGroupEndDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newGroupEndDate);
                if (!isTrueFormat)
                {
                    Messages.InvalidInputMessage("new group end date");
                    goto EnterNewGroupEndDate;
                }

                if (existGroup.BeginDate.Date.AddMonths(6).Date > newGroupEndDate.Date)
                {
                    Messages.InvalidInputMessage("End Date");
                    goto EnterNewGroupEndDate;
                }
            }
        }

        if (!string.IsNullOrEmpty(newGroupName)) { existGroup.Name = newGroupName; }
        if (newGroupLimit != existGroup.Limit) { existGroup.Limit = existGroup.Limit; }
        if (newGroupBeginDate != existGroup.BeginDate) { existGroup.BeginDate = newGroupBeginDate; }
        if (newGroupEndDate != existGroup.EndDate) { existGroup.EndDate = newGroupEndDate; }

        _unitOfWork.Commit();


        Messages.SuccessMessage("Group", "updated");
    }

    public  void DeleteGroup()
    {
        GetAllGroups();
    InputIdLine: Messages.InputMessage("group Id");
        var inputId = Console.ReadLine();
        int Id;
        bool isTrueIdFormat = int.TryParse(inputId, out Id);
        if (!isTrueIdFormat)
        {
            Messages.InvalidInputMessage("Group ID");
            goto InputIdLine;
        }

        var group = _unitOfWork.Groups.GetById(Id);
        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            return;
        }

        _unitOfWork.Groups.Delete(group);
        _unitOfWork.Commit();

        Messages.SuccessMessage("Group", "deleted");
    }






}
