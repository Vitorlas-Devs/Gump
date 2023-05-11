// https://github.com/vuejs/pinia/discussions/1324#discussioncomment-2833464

import type { StateTree, Store } from 'pinia'

type PiniaStateTree = StateTree
type PiniaGetterTree = Record<string, (...args: any) => any>
type PiniaActionTree = Record<string, (...args: any) => any>

type PickState<TStore extends Store> = TStore extends Store<string, infer TState, PiniaGetterTree, PiniaActionTree> ? TState : PiniaStateTree
type PickActions<TStore extends Store> = TStore extends Store<string, PiniaStateTree, PiniaGetterTree, infer TActions> ? TActions : never
type PickGetters<TStore extends Store> = TStore extends Store<string, PiniaStateTree, infer TGetters, PiniaActionTree> ? TGetters : never

type CompatiblePiniaState<TState> = () => TState
type CompatiblePiniaGetter<TGetter extends (...args: any) => any, TStore extends Store> = (this: TStore, state: PickState<TStore>) => ReturnType<TGetter>
type CompatiblePiniaGetters<TGetters extends PiniaGetterTree, TStore extends Store> = {
  [Key in keyof TGetters]: CompatiblePiniaGetter<TGetters[Key], CompatibleStore<TStore>>;
}

type CompatiblePiniaAction<TAction extends (...args: any) => any, TStore extends Store> = (this: TStore, ...args: Parameters<TAction>) => ReturnType<TAction>
type CompatiblePiniaActions<TActions extends PiniaActionTree, TStore extends Store> = {
  [Key in keyof TActions]: CompatiblePiniaAction<TActions[Key], CompatibleStore<TStore>>;
}

type CompatibleStore<TStore extends Store> = TStore extends Store<string, infer TState, infer TGetters, infer TActions> ? Store<string, TState, TGetters, TActions> : never

type PiniaState<TStore extends Store> = CompatiblePiniaState<PickState<TStore>>
type PiniaGetters<TStore extends Store> = CompatiblePiniaGetters<PickGetters<TStore>, TStore>
type PiniaActions<TStore extends Store> = CompatiblePiniaActions<PickActions<TStore>, TStore>
type PiniaStore<TStore extends Store> = {
  state: PiniaState<TStore>
  getters: PiniaGetters<TStore>
  actions: PiniaActions<TStore>
}
