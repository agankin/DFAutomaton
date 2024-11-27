# DFAutomaton

DFAutomaton is a library for building State Automata.

### Automaton model

An automaton works on a set of states interconnected by state transitions forming a graph.

A state transition can be:

- Fixed transition with known next state.
- Dynamic transition with the next state determined dynamicly in a reducer function.
- Fallback transition is an optional per-state single fixed or dynamic transition invoked when no other transition found.  

Selection of a state transition can be:

- By equality to some transition value.
- By checking a transition predicate.
- By default when no transition found. It can be configured for only one transition per state.

To be run an automaton takes 2 parameters:

- Some initial value.
- A sequence of transitions.

Automaton starts with an initial value from the start state and finishes reaching the accepted state with some resulting value. On each transition automaton's current value is transformed by invoking the related reducer function.

Additional features present to make implementation of some algorithms easier:

- Possibility of dynamicly pushing new transition values as next inside reducers.
- Registering a predicate for checking state value for errors.

Automaton execution can result in errors:

- A transition from a state does not exist.
- An attempt of transitioning from the accepted state is made.
- No next state is determined for a dynamic transition.
- The accepted state was not reached after all transitions performed.
- An automaton state is determined to be an error state.