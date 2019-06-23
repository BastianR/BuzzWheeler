using BrElements.Classes;
using Prism.Events;
using System.Collections.ObjectModel;

namespace BuzzWheeler.AggregatorEvents
{
    public class MessageBrCircleItemsEvent : PubSubEvent<ObservableCollection<BrArcCircleItem>> { }
}
