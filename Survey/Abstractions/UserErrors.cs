namespace Survey.Abstractions
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentionals =
            new Error("User.InvalidCredentionals", "Invalid Username or password" , StatusCodes.Status401Unauthorized);
    }
    public class PollErrors
    {
        public static readonly Error PollNotFound =
            new Error("Poll.NotFound", "No poll was found by this id", StatusCodes.Status404NotFound);
        public static readonly Error DuplicatedPoll =
            new Error("Duplicate Content", "there is an exixted Poll has the same Tittle", StatusCodes.Status409Conflict);
    }
    public class TokenErrors
    {
        public static readonly Error EmptyToken =
            new Error("NotFound", "Null Refrence", StatusCodes.Status404NotFound);
    }
    public class DuplicateTittleErrors
    {
        public static readonly Error DuplicatedTittle =
            new Error("Duplicate tittle", "there is an exixted poll has the same tittle" , StatusCodes.Status409Conflict);
    }
    public class DuplicateQuestionContent
    {
        public static readonly Error DuplicatedContent =
            new Error("Duplicate Content", "there is an exixted question has the same content", StatusCodes.Status409Conflict);
    }
    public class QuestionError
    {
        public static readonly Error QuestionNotFound =
            new Error("Question.NotFound", "No Question was found by this id" , StatusCodes.Status404NotFound);
    }
    public class VoteError
    {
        public static readonly Error DuplicatedVoting =
            new Error("Vote.DuplicateVote", "This User had voted before" , StatusCodes.Status409Conflict);
        public static readonly Error InvalidQuestion =
            new Error("Vote.InvaildQuestion", "InvalidQuestions", StatusCodes.Status400BadRequest);
    }
}
