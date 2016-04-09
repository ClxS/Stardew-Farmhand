namespace Farmhand.API
{
    public class RequiredSkill
    {
        public string Skill;
        public int Level;

        public override string ToString()
        {
            return $"{Skill} {Level}";
        }
    }
}