namespace Application.Services.Abstract;

public interface IStudentService
{
    void GetAllStudents();
    void GetStudent();
    void AddStudent();
    void UpdateStudent();
    void DeleteStudent();
}