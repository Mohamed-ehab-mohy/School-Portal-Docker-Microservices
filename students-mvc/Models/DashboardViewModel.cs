namespace students_mvc.Models;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalClasses { get; set; }
    public int TotalGrades { get; set; }

    public int StudentsPresentToday { get; set; }
    public int StudentsAbsentToday { get; set; }
    public int StudentsLateToday { get; set; }
    public double AttendanceRateToday { get; set; }

    public double AverageScore { get; set; }
    public double HighestScore { get; set; }
    public double LowestScore { get; set; }

    public List<ClassStatistics> ClassStats { get; set; } = [];
    public List<SubjectStatistics> SubjectStats { get; set; } = [];
    public List<MonthlyAttendance> MonthlyAttendance { get; set; } = [];
    public List<TeacherWorkload> TeacherWorkloads { get; set; } = [];
}

public class ClassStatistics
{
    public string ClassName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public double AverageScore { get; set; }
    public double AttendanceRate { get; set; }
}

public class SubjectStatistics
{
    public string Subject { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public int GradeCount { get; set; }
}

public class MonthlyAttendance
{
    public string Month { get; set; } = string.Empty;
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Late { get; set; }
}

public class TeacherWorkload
{
    public string TeacherName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int ClassCount { get; set; }
    public int StudentCount { get; set; }
}
