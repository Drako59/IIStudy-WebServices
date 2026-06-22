using LLstudyWS.ORM.CreatorsModels;

namespace LLstudyWS.ORM.Repositorys
{
    public class RepositoryUOW
    {
        BookRepository bookRepository;
        SubjectRepository subjectRepository;
        OrderRepository orderRepository;
        ReviewRepository reviewRepository;
        ShoppingCartRepository shoppingCartRepository;
        SolutionRepository solutionRepository;
        EventRepository eventRepository;
        ExamRepository examRepository;
        RegisteredRepository registeredRepository;
        DbHelperOledb helperOledb;
        ModelCreatorReflection modelCreatorReflection;
        LikeRepository likeRepository;

        public RepositoryUOW()
        {
            this.modelCreatorReflection = new ModelCreatorReflection();
            this.helperOledb = new DbHelperOledb();
        }

        public BookRepository BookRepository { get { 
                
                if (bookRepository == null) {
                    this.bookRepository = new BookRepository(this.helperOledb, this.modelCreatorReflection);
                }
                return bookRepository;
            }
        }
        public SubjectRepository SubjectRepository
        { get {
                if (subjectRepository == null)
                {
                    this.subjectRepository = new SubjectRepository(this.helperOledb, this.modelCreatorReflection);
                }
                return subjectRepository;
            } }
        public OrderRepository OrderRepository { get
            {
                if (orderRepository == null)
                    this.orderRepository = new OrderRepository(this.helperOledb,  this.modelCreatorReflection);
                return this.orderRepository;
            } }

        public ReviewRepository ReviewRepository
        {
            get {if (this.reviewRepository == null)
                    this.reviewRepository = new ReviewRepository(this.helperOledb,  this.modelCreatorReflection);
                return reviewRepository; }
        }

        public EventRepository EventRepository
        {
            get
            {
                if (this.eventRepository == null)
                    this.eventRepository = new EventRepository(this.helperOledb,  this.modelCreatorReflection);
                return this.eventRepository;
            }
        }
        public ExamRepository ExamRepository
        {
            get
            {
                if (this.examRepository == null)
                    this.examRepository = new ExamRepository(this.helperOledb,  this.modelCreatorReflection);
                return this.examRepository;
            }
        }
        public SolutionRepository SolutionRepository
        {
            get
            {
                if (this.solutionRepository == null)
                    this.solutionRepository = new SolutionRepository(this.helperOledb,  this.modelCreatorReflection);
                return this.solutionRepository;
            }
        }

        public RegisteredRepository RegisteredRepository {
            get
            {
                if (this.registeredRepository == null)
                    this.registeredRepository = new RegisteredRepository(this.helperOledb,  this.modelCreatorReflection);
                return this.registeredRepository;
            }
        }
        public ShoppingCartRepository ShoppingCartRepository
        {
            get
            {
                if (this.shoppingCartRepository == null)
                    this.shoppingCartRepository = new ShoppingCartRepository(this.helperOledb, this.modelCreatorReflection);
                return this.shoppingCartRepository;
            }
        }

        public LikeRepository LikeRepository
        {
            get
            {
                if (this.likeRepository == null)
                    this.likeRepository = new LikeRepository(this.helperOledb,this.modelCreatorReflection);
                return this.likeRepository;
            }
        }

        public DbHelperOledb HelperOledb { get { return this.helperOledb; } }
    }
}
