using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bolnica.Commands
{
	class SekretarCommands
	{
		public static readonly ICommand DodajObavestenjeCommand;
		public static readonly ICommand DodajPacijentaCommand;
		public static readonly ICommand DodajGostujucegPacijentaCommand;
		public static readonly ICommand DodajTerminCommand;
		public static readonly ICommand DodajLekaraCommand;
		public static readonly ICommand DodajTerminPacijentuCommand;
        public static readonly ICommand DodajTerminLekaruCommand;
        public static readonly ICommand DodajIzvestajCommand;

		public static readonly ICommand IzmeniObavestenjeCommand;
		public static readonly ICommand IzmeniPacijentaCommand;
		public static readonly ICommand IzmeniTerminCommand;
		public static readonly ICommand IzmeniLekaraCommand;
		public static readonly ICommand IzmeniProfilCommand;
		public static readonly ICommand IzmeniLozinkuCommand;

		public static readonly ICommand ObrisiObavestenjeCommand;
		public static readonly ICommand ObrisiPacijentaCommand;
		public static readonly ICommand ObrisiTerminCommand;
		public static readonly ICommand ObrisiLekaraCommand;

		public static readonly ICommand PogledajObavestenjeCommand;
		public static readonly ICommand RadnoVremeLekaraCommand;
        public static readonly ICommand FeedbackCommand;

        public static readonly ICommand SelectObavestenjeCommand;
        public static readonly ICommand SelectPacijentaCommand;
        public static readonly ICommand SelectTerminCommand;
        public static readonly ICommand SelectLekaraCommand;

		static SekretarCommands()
		{
			var inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.O }, ModifierKeys.Control, "Ctrl+A, O"));
			DodajObavestenjeCommand = new RoutedUICommand("dodajObaDodajObavestenjeCommandvestenje", "DodajObavestenjeCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.P }, ModifierKeys.Control, "Ctrl+A, P"));
			DodajPacijentaCommand = new RoutedUICommand("DodajPacijentaCommand", "DodajPacijentaCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.G }, ModifierKeys.Control, "Ctrl+A, G"));
			DodajGostujucegPacijentaCommand = new RoutedUICommand("DodajGostujucegPacijentaCommand", "DodajGostujucegPacijentaCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.T }, ModifierKeys.Control, "Ctrl+A, T"));
			DodajTerminCommand = new RoutedUICommand("DodajTerminCommand", "DodajTerminCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.L }, ModifierKeys.Control, "Ctrl+A, L"));
			DodajLekaraCommand = new RoutedUICommand("DodajLekaraCommand", "DodajLekaraCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.T, Key.P }, ModifierKeys.Control, "Ctrl+T, P"));
			DodajTerminPacijentuCommand = new RoutedUICommand("DodajTerminPacijentuCommand", "DodajTerminPacijentuCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.T, Key.L }, ModifierKeys.Control, "Ctrl+T, L"));
            DodajTerminLekaruCommand = new RoutedUICommand("DodajTerminLekaruCommand", "DodajTerminLekaruCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.I }, ModifierKeys.Control, "Ctrl+A, I"));
            DodajIzvestajCommand = new RoutedUICommand("DodajIzvestajCommand", "DodajIzvestajCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.O }, ModifierKeys.Control, "Ctrl+E, O"));
			IzmeniObavestenjeCommand = new RoutedUICommand("IzmeniObavestenjeCommand", "IzmeniObavestenjeCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.P }, ModifierKeys.Control, "Ctrl+E, P"));
			IzmeniPacijentaCommand = new RoutedUICommand("IzmeniPacijentaCommand", "IzmeniPacijentaCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.T }, ModifierKeys.Control, "Ctrl+E, T"));
			IzmeniTerminCommand = new RoutedUICommand("IzmeniTerminCommand", "IzmeniTerminCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.L }, ModifierKeys.Control, "Ctrl+E, L"));
			IzmeniLekaraCommand = new RoutedUICommand("IzmeniLekaraCommand", "IzmeniLekaraCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.S }, ModifierKeys.Control, "Ctrl+E, S"));
			IzmeniProfilCommand = new RoutedUICommand("IzmeniProfilCommand", "IzmeniProfilCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.E, Key.Z }, ModifierKeys.Control, "Ctrl+E, Z"));
			IzmeniLozinkuCommand = new RoutedUICommand("IzmeniLozinkuCommand", "IzmeniLozinkuCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.D, Key.O }, ModifierKeys.Control, "Ctrl+D, O"));
			ObrisiObavestenjeCommand = new RoutedUICommand("ObrisiObavestenjeCommand", "ObrisiObavestenjeCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.D, Key.P }, ModifierKeys.Control, "Ctrl+D, P"));
			ObrisiPacijentaCommand = new RoutedUICommand("ObrisiPacijentaCommand", "ObrisiPacijentaCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.D, Key.T }, ModifierKeys.Control, "Ctrl+D, T"));
			ObrisiTerminCommand = new RoutedUICommand("ObrisiTerminCommand", "ObrisiTerminCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.D, Key.L }, ModifierKeys.Control, "Ctrl+D, L"));
			ObrisiLekaraCommand = new RoutedUICommand("ObrisiLekaraCommand", "ObrisiLekaraCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.V, Key.O }, ModifierKeys.Control, "Ctrl+V, O"));
			PogledajObavestenjeCommand = new RoutedUICommand("PogledajObavestenjeCommand", "PogledajObavestenjeCommand", typeof(SekretarCommands), inputGestures);

			inputGestures = new InputGestureCollection();
			inputGestures.Add(new MultiKeyGesture(new Key[] { Key.R, Key.L }, ModifierKeys.Control, "Ctrl+R, L"));
			RadnoVremeLekaraCommand = new RoutedUICommand("RadnoVremeLekaraCommand", "RadnoVremeLekaraCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.A, Key.F }, ModifierKeys.Control, "Ctrl+A, F"));
            FeedbackCommand = new RoutedUICommand("FeedbackCommand", "FeedbackCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.O, Key.O }, ModifierKeys.Control, "Ctrl+O, O"));
            SelectObavestenjeCommand = new RoutedUICommand("SelectObavestenjeCommand", "SelectObavestenjeCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.O, Key.P }, ModifierKeys.Control, "Ctrl+O, P"));
            SelectPacijentaCommand = new RoutedUICommand("SelectPacijentaCommand", "SelectPacijentaCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.O, Key.T }, ModifierKeys.Control, "Ctrl+O, T"));
            SelectTerminCommand = new RoutedUICommand("SelectTerminCommand", "SelectTerminCommand", typeof(SekretarCommands), inputGestures);

            inputGestures = new InputGestureCollection();
            inputGestures.Add(new MultiKeyGesture(new Key[] { Key.O, Key.L }, ModifierKeys.Control, "Ctrl+O, L"));
            SelectLekaraCommand = new RoutedUICommand("SelectLekaraCommand", "SelectLekaraCommand", typeof(SekretarCommands), inputGestures);
		}
	}

	/*
	 *
	 * REFERENCA: https://kent-boogaart.com/blog/multikeygesture
	 *
	 */

	public class MultiKeyGesture : KeyGesture
	{
		private readonly IList<Key> _keys;
		private readonly ReadOnlyCollection<Key> _readOnlyKeys;
		private int _currentKeyIndex;
		private DateTime _lastKeyPress;
		private static readonly TimeSpan MaximumDelayBetweenKeyPresses = TimeSpan.FromSeconds(1);

		public MultiKeyGesture(IEnumerable<Key> keys, ModifierKeys modifiers)
			: this(keys, modifiers, string.Empty)
		{
		}

		public MultiKeyGesture(IEnumerable<Key> keys, ModifierKeys modifiers, string displayString)
			: base(Key.None, modifiers, displayString)
		{
			_keys = new List<Key>(keys);
			_readOnlyKeys = new ReadOnlyCollection<Key>(_keys);

			if (_keys.Count == 0)
			{
				throw new ArgumentException("At least one key must be specified.", "keys");
			}
		}

		public ICollection<Key> Keys
		{
			get { return _readOnlyKeys; }
		}

		public override bool Matches(Object targetElement, InputEventArgs inputEventArgs)
		{
			var args = inputEventArgs as KeyEventArgs;

			if ((args == null) || !IsDefinedKey(args.Key))
			{
				return false;
			}

			if (_currentKeyIndex != 0 && ((DateTime.Now - _lastKeyPress) > MaximumDelayBetweenKeyPresses))
			{
				//took too long to press next key so reset
				_currentKeyIndex = 0;
				return false;
			}

			//the modifier only needs to be held down for the first keystroke, but you could also require that the modifier be held down for every keystroke
			if (_currentKeyIndex == 0 && Modifiers != Keyboard.Modifiers)
			{
				//wrong modifiers
				_currentKeyIndex = 0;
				return false;
			}

			if (_keys[_currentKeyIndex] != args.Key)
			{
				//wrong key
				_currentKeyIndex = 0;
				return false;
			}

			++_currentKeyIndex;

			if (_currentKeyIndex != _keys.Count)
			{
				//still matching
				_lastKeyPress = DateTime.Now;
				inputEventArgs.Handled = true;
				return false;
			}

			//match complete
			_currentKeyIndex = 0;
			return true;
		}

		private static bool IsDefinedKey(Key key)
		{
			return ((key >= Key.None) && (key <= Key.OemClear));
		}
	}
}