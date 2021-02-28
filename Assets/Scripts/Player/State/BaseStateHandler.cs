using UnityEngine;

using System;
using System.Collections.Generic;

using NaughtyAttributes;

public abstract class BaseStateHandler<TEnum> : MonoBehaviour where TEnum: Enum {
	[SerializeField] [ReadOnly] TEnum _state;

	readonly Dictionary<TEnum, List<TEnum>> _transitions = new Dictionary<TEnum, List<TEnum>>();

	protected TEnum State => _state;
	
	protected bool SetState(TEnum newState) {
		if ( !CheckTransition(_state, newState) ) {
			return false;
		}

		var prevState = _state;
		_state = newState;
		OnChangedState(prevState, _state);
		return true;
	}

	protected void AddTransition(TEnum from, TEnum to) {
		if ( !_transitions.ContainsKey(from) ) {
			_transitions[from] = new List<TEnum>();
		}
		
		if ( _transitions[from].Contains(to) ) {
			return;
		}
		
		_transitions[from].Add(to);
	}

	protected virtual void OnChangedState(TEnum from, TEnum to) {
	}

	bool CheckTransition(TEnum from, TEnum to) {
		return _transitions.ContainsKey(from) && _transitions[from].Contains(to);
	}
}
