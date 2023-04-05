namespace AutotestWeb.Models;

public class QuestionModel
{
    public long Id;
    public string Question;
    public List<Choices> Choices;
    public Media Media;
    public string Description;
}
