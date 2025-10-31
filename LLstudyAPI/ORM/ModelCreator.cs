using System.Reflection.Metadata.Ecma335;

namespace LLstudyWS.ORM
{
    public class ModelCreators
    {
        BookCreator bookCreator;
        ExamCreator examCreator;
        ShoppingCartCreator shoppingCartCreator;
        SolutionCreator solutionCreator;
        OrderCreator orderCreator;  
        ReviewCreator reviewCreator;
        RegisterCreator registerCreator;
        EventCreator    eventCreator;   
        CategoryCreator categoryCreator;

        public BookCreator BookCreator { get 
            {
                if (this.bookCreator == null)
                    this.bookCreator = new BookCreator();
                return this.bookCreator;
            }
        }
        public ExamCreator ExamCreator { get 
            {
                if (this.examCreator == null)
                    this.examCreator = new ExamCreator();   
                return this.examCreator;
            }
        }
        public ShoppingCartCreator ShoppingCartCreator {  get
            {
                if (this.shoppingCartCreator == null)
                    this.shoppingCartCreator = new ShoppingCartCreator();
                return this.shoppingCartCreator;
            }
        }
        public SolutionCreator SolutionCreator { get 
            {
                if (this.solutionCreator == null)
                    this.solutionCreator = new SolutionCreator();
                return this.solutionCreator;
            }
        }
        public OrderCreator OrderCreator { get 
            {
                if (this.orderCreator == null)
                    this.orderCreator = new OrderCreator();
                return this.orderCreator;
            }
        }
        public ReviewCreator ReviewCreator { get 
            {
                if (this.reviewCreator == null)
                    this.reviewCreator = new ReviewCreator();   
                return this.reviewCreator;
            }
        }
        public RegisterCreator RegisterCreator { get
            {
                if (this.registerCreator == null)
                    this.registerCreator = new RegisterCreator();
                return this.registerCreator;

            }

        }
        public EventCreator EventCreator { 
            
            get
            {
                if (this.eventCreator != null)
                    this.eventCreator = new EventCreator();
                return this.EventCreator;
            }
        }
        public CategoryCreator CategoryCreator { get 
            {
                if (this.categoryCreator == null)
                    this.categoryCreator = new CategoryCreator();
                return this.categoryCreator;
            }
        }


    }
}
