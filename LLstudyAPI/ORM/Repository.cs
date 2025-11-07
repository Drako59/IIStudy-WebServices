namespace LLstudyWS.ORM
{
    public class Repository
    {
        protected DbHelperOledb helperOledb;
        protected ModelCreators modelCreators;

        public Repository()
        {
            this.modelCreators = new ModelCreators();
            this.helperOledb = new DbHelperOledb();
        }
    }
}
