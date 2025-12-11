using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM.Repositorys
{
    public class RepositoryUOW
    {
        BookRepository bookRepository;
        CategoryRepository categoryRepository;
        OrderRepository orderRepository;
        ReviewRepository reviewRepository;
        ShoppingCartRepository shoppingCartRepository;
        SolutionRepository solutionRepository;
        EventRepository eventRepository;
        ExamRepository examRepository;
        RegisteredRepository registeredRepository;
        DbHelperOledb helperOledb;
        ModelCreators modelCreators;
        ModelCreatorReflection modelCreatorReflection;


        public RepositoryUOW()
        {
            this.modelCreatorReflection = new ModelCreatorReflection();
            this.modelCreators = new ModelCreators();
            this.helperOledb = new DbHelperOledb();
        }

        public BookRepository BookRepository { get { 
                
                if (bookRepository == null) {
                    this.bookRepository = new BookRepository(this.helperOledb, this.modelCreators,this.modelCreatorReflection);
                }
                return bookRepository;
            }
        }
        public CategoryRepository CategoryRepository { get {
                if (categoryRepository == null)
                {
                    this.categoryRepository = new CategoryRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                }
                return categoryRepository;
            } }
        public OrderRepository OrderRepository { get
            {
                if (orderRepository == null)
                    this.orderRepository = new OrderRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.orderRepository;
            } }

        public ReviewRepository ReviewRepository
        {
            get {if (this.reviewRepository == null)
                    this.reviewRepository = new ReviewRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return reviewRepository; }
        }

        public EventRepository EventRepository
        {
            get
            {
                if (this.eventRepository == null)
                    this.eventRepository = new EventRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.eventRepository;
            }
        }
        public ExamRepository ExamRepository
        {
            get
            {
                if (this.examRepository == null)
                    this.examRepository = new ExamRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.examRepository;
            }
        }
        public SolutionRepository SolutionRepository
        {
            get
            {
                if (this.solutionRepository == null)
                    this.solutionRepository = new SolutionRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.solutionRepository;
            }
        }

        public RegisteredRepository RegisteredRepository {
            get
            {
                if (this.registeredRepository == null)
                    this.registeredRepository = new RegisteredRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.registeredRepository;
            }
        }
        public ShoppingCartRepository ShoppingCartRepository
        {
            get
            {
                if (this.shoppingCartRepository == null)
                    this.shoppingCartRepository = new ShoppingCartRepository(this.helperOledb, this.modelCreators, this.modelCreatorReflection);
                return this.shoppingCartRepository;
            }
        }

        public DbHelperOledb HelperOledb { get { return this.helperOledb; } }
    }
}
