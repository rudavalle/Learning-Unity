using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriviaGameLoop : MonoBehaviour {

    //This is the struct that sets up what the question values are going to be

    public struct Question
	{
		public string questionText;
		public string[] answers;
		public int correctAnswersIndex;

		public Question(string questionText, string[] answers, int correctAnswersIndex)
		{
			this.questionText = questionText;
			this.answers = answers;
			this.correctAnswersIndex = correctAnswersIndex;
		}

	}

	private Question currentQuestion = new Question("What is your favorite color?",new string[]{"blue","red","yellow","white","black"},0);
	public Button[] answerButtons;
    public Text questionText; 

    private Question[] questions = new Question[10];
    private int currentQuestionIndex;
    private int[] questionNumbersChoosen = new int[5];
    private int questionsFinished;

    public GameObject[] triviaPannels;
    public GameObject finalResultsPanel;
    public Text resultsText;
    private int numberOfCorrectAnswers;
    private bool allowSelection = true;
    public GameObject feedbackText;


    //Use this for initialization
    void Start()
	{
        for (int i = 0; i < questionNumbersChoosen.Length; i++)
        {
            questionNumbersChoosen[i] = -1;
        }

        questions[0] = new Question("What is the capital of Spain?", new string[] { "Topeka", "Amsterdam", "Madrid", "London", "Toledo" }, 2);
        questions[1] = new Question("Who was the second US president?", new string[] { "Thomas Jefferson", "John Adams", "Bill Clinton", "George Washington", "Abraham Lincoln" }, 1);
        questions[2] = new Question("What is the second planet in our solar system?", new string[] { "Mercury", "Earth", "Saturn", "Venus", "Pluto" }, 3);
        questions[3] = new Question("What is the largest continent?", new string[] { "Africa", "North America", "Asia", "Europe", "Australia" }, 2);
        questions[4] = new Question("What US state has the largest population?", new string[] { "California", "Florida", "Texas", "New York", "North Carolina" }, 0);
        questions[5] = new Question("A platypus is a ________", new string[] { "Bird", "Reptile", "Insect", "Amphibian", "Mammal" }, 4);
        questions[6] = new Question("What is the boiling temperature in fahrenheit?", new string[] { "100 degrees", "190 degrees", "200 degrees", "312 degrees", "212 degrees" }, 4);
        questions[7] = new Question("How many degrees are in a circle?", new string[] { "360", "180", "640", "16", "270" }, 0);
        questions[8] = new Question("What is a name for a group of crows?", new string[] { "A bloat", "A herd", "A pack", "A murder", "A team" }, 3);
        questions[9] = new Question("Who created the painting starry night?", new string[] { "Pablo Picasso", "Vincent Van Gogh", "Andy Warhol", "Leonardo da Vinci", "Frida Kahlo" }, 1);

        chooseQuestions();

        assignQuestion(questionNumbersChoosen[0]);
    }




	//Update is called once for frame
	void update()
	{
		
	}


    //Setting up the interface to show a question

	void assignQuestion(int questionNum)
	{
        currentQuestion = questions[questionNum];
		questionText.text = currentQuestion.questionText;
		for (int i = 0; i < answerButtons.Length; i++) 
		{

			answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
		}
	}

    public void checkAnswer(int buttonNum)
    {
        if (allowSelection)
        {
            if (buttonNum == currentQuestion.correctAnswersIndex)
            {
                print("CORRECT");
                numberOfCorrectAnswers++;
                feedbackText.GetComponent<Text>().text = "Correct";
                feedbackText.GetComponent<Text>().color = Color.green;
            }
            else
            {
                print("INCORRECT");
                feedbackText.GetComponent<Text>().text = "Incorrect";
                feedbackText.GetComponent<Text>().color = Color.red;
            }
            StartCoroutine("continueAfterFeedback");
        }

        

    }

    void chooseQuestions()
    {
        for (int i = 0; i < questionNumbersChoosen.Length; i++)
        {

            int questionNum = Random.Range(0,questions.Length);

            if (numberNotContained(questionNumbersChoosen, questionNum))
            {
                questionNumbersChoosen[i] = questionNum;
            }

            else {
                i--;
            }

        }
        currentQuestionIndex = Random.Range(0, questions.Length);

        

    }

    bool numberNotContained(int[] numbers,int num)
    {
        for (int i = 0; i <numbers.Length; i++)

        {
            if(num == numbers[i])
            {
                return false;
            }
        }

        return true;
    }


    public void moveToNextQuestion()
    {
        assignQuestion(questionNumbersChoosen[questionNumbersChoosen.Length - 1 - questionsFinished]);
    }

    void displayResults()
    {

        switch (numberOfCorrectAnswers)

        {

            case 5:

                resultsText.text = "5 out of 5 correct. You are all knowing!";

                break;

            case 4:

                resultsText.text = "4 out of 5 correct. You are very smart!";

                break;

            case 3:

                resultsText.text = "3 out of 5 correct. Well done.";

                break;

            case 2:

                resultsText.text = "2 out of 5 correct. Better luck next time.";

                break;

            case 1:

                resultsText.text = "1 out of 5 correct. You can do better than that!";

                break;

            case 0:

                resultsText.text = "0 out of 5 correct. Are you even trying?";

                break;

            default:

                print("Incorrect intelligence level.");

                break;

        }

    }

    public void restartLevel()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
   
    IEnumerator continueAfterFeedback()
    {
        allowSelection = false;
        feedbackText.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        if (questionsFinished < questionNumbersChoosen.Length - 1)
        {
            moveToNextQuestion();
            questionsFinished++;
        }
        else
        {
            foreach (GameObject p in triviaPannels)
            {
                p.SetActive(false);
            }

            finalResultsPanel.SetActive(true);
            displayResults();
        }

        allowSelection = true;
        feedbackText.SetActive(false);

    }

}