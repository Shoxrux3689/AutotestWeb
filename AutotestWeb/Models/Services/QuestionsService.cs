using Newtonsoft.Json;

namespace AutotestWeb.Models.Services
{
    public static class QuestionsService
    {
        public static List<QuestionModel> Questions;
        public static List<QuestionModel> ReadQuestion(string language)
        {
            var question = new List<QuestionModel>();
            if (language == "lotin")
            {
                var json = File.ReadAllText("JsonData/uzlotin.json");
                question = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
            }
            else if (language == "kiril")
            {
                var json1 = File.ReadAllText("JsonData/uzkiril.json");
                question = JsonConvert.DeserializeObject<List<QuestionModel>>(json1);
            }
            else
            {
                var json2 = File.ReadAllText("JsonData/rus.json");
                question = JsonConvert.DeserializeObject<List<QuestionModel>>(json2);
            }
            return question;
        }
    }
}
