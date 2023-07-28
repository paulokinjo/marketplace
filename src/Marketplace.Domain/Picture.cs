using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        internal ClassifiedAdId ParentId { get; private set; }
        internal PictureSize Size { get; private set; }
        internal Uri Location { get; private set; }
        internal int Order { get; private set; }


        public Picture()
        {

        }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    ParentId = new ClassifiedAdId(e.ClassifiedAdId);
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    break;
            }
        }

        public void Resize(PictureSize newSize)
            => Apply(new Events.ClassifiedAdPictureResized
            {
                PictureId = Id.Value,
                ClassifiedAdId = ParentId,
                Height = newSize.Width,
                Width = newSize.Width
            });

        public Picture(Action<object> applier) : base(applier)
        {
        }
    }

    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;

        public Guid Value { get; }
    }

    public class PictureSize : Value<PictureSize>
    {
        public int Width { get; internal set; }
        public int Height { get; internal set; }

        public PictureSize(int width, int height)
        {
            if (Width <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(width),
                    "Picture width must be a positive number");
            }

            if (Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(height),
                    "Picture height must be a positive number");
            }
            Width = width;
            Height = height;
        }

        internal PictureSize()
        {

        }
    }
}
