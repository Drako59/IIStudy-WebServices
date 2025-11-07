namespace LLstudyWS.ORM.Repositorys
{
    public class RepositoryUOW
    {
        BookRepositery bookRepository;
        CategoryRepository categoryRepository;
        OrderRepository orderRepository;
        ReviewRepository reviewRepository;
        ShoppingCartRepository shoppingCartRepository;
        SolutionRepository solutionRepository;
        EventRepository eventRepository;
        ExamRepository examRepository;
        RegisterCreator registerCreator;


        public BookRepositery BookRepository { get { 
                
                if (bookRepository == null) {
                    this.bookRepository = new BookRepositery();
                }
                return bookRepository;
            }
        }
        public CategoryRepository CategoryRepository { get {
                if (categoryRepository == null)
                {
                    this.categoryRepository = new CategoryRepository();
                }
                return categoryRepository;
            } }
        public OrderRepository OrderRepository { get
            {
                if (orderRepository == null)
                    this.orderRepository = new OrderRepository();
                return this.orderRepository;
            } }

        public ReviewRepository ReviewRepository
        {
            get {if (this.reviewRepository == null)
                    this.reviewRepository = new ReviewRepository();
                return reviewRepository; }
        }
    }
}
