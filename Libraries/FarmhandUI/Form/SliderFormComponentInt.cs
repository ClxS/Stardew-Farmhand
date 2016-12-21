namespace Farmhand.UI.Form
{
    using System.Linq;

    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The slider form component.
    /// </summary>
    public class SliderFormComponent : SliderFormComponent<int>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="steps">
        ///     The maximum value of the slider
        /// </param>
        /// <param name="handler">
        ///     The value changed handler
        /// </param>
        public SliderFormComponent(Point position, int steps, ValueChanged<int> handler = null)
            : this(position, 100, steps, handler)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SliderFormComponent" /> class.
        /// </summary>
        /// <param name="position">
        ///     The position of this component
        /// </param>
        /// <param name="width">
        ///     The width of this slider
        /// </param>
        /// <param name="steps">
        ///     The maximum value of the slider
        /// </param>
        /// <param name="handler">
        ///     The value changed handler
        /// </param>
        public SliderFormComponent(Point position, int width, int steps, ValueChanged<int> handler = null)
            : base(position, width, Enumerable.Range(0, steps).ToList(), handler)
        {
        }
    }
}