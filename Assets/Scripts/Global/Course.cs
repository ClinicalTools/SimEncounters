using System.Collections.Generic;

public class Course
{
	string title;
	string description;
	string author;
	List<string> cases;

	public List<string> GetCases()
	{
		return cases;
	}

	public Course(string cTitle, string cDescription, string cAuthor, List<string> caseList)
	{
		title = cTitle; description = cDescription; author = cAuthor; cases = caseList;
	}

	public void AddCourse(string recordNumber)
	{
		cases.Add(recordNumber);
	}

	public bool RemoveCourse(string recordNumber)
	{
		return cases.Remove(recordNumber);
	}

	public bool HasCourse(string recordNumber)
	{
		return cases.Contains(recordNumber);
	}

	public string GetTitle()
	{
		return title;
	}

	public string GetDescription()
	{
		return description;
	}

	public string GetAuthor()
	{
		return author;
	}

	/// <summary>
	/// "~~" separated values representing a course
	/// </summary>
	/// <returns>title, description, author, courses</returns>
	public override string ToString()
	{
		return string.Join("--", title, description, author, string.Join("-", cases));
	}
}
