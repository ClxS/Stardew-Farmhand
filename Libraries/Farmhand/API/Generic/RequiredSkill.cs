namespace Farmhand.API.Generic
{
    /// <summary>
    ///     Used to define a skill requirement.
    /// </summary>
    public class RequiredSkill
    {
        /// <summary>
        ///     Gets or sets the skill level required.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the skill required.
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        ///     Converts the requirement into a game compatible string.
        /// </summary>
        /// <returns>
        ///     The requirement as a string.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Skill} {this.Level}";
        }
    }
}