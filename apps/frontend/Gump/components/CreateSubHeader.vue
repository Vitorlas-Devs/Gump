<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

defineProps<{
  variant: 'ingredients' | 'steps'
}>()

const ui = useUIStore()

const toggled = ref(false)

function togglePanel() {
  toggled.value = !toggled.value
}

ui.createMode = 'design'

function toggleMode() {
  ui.createMode = ui.createMode === 'raw' ? 'design' : 'raw'
}
</script>

<template>
  <div
    flex="~ row" h-14 w-full items-center justify-between px-10 font-bold
  >
    <p v-if="!toggled" flex-1 text-center text-xl>
      {{ variant === 'ingredients' ? $t('CreateIngredients') : $t('CreateSteps') }}
    </p>
    <div
      v-else-if="variant === 'steps'"
      flex-1 text-center text-lg
    >
      {{ $t('CreateStepsTip') }}

      <!-- this should work but no -->
      <!-- <i18n path="CreateStepsTip" tag="p">
        <template #color>
          <span text-orange-500 text-shadow-orange>
            {{ $t('CreateStepsTipOrange') }}
          </span>
        </template>
      </i18n> -->
    </div>
    <div
      v-else
      flex="~ row" flex-1 items-center justify-center text-center text-xl
    >
      <p
        :class="ui.createMode === 'design' ? 'mode design-active text-white-500 text-shadow-white' : 'mode design'"
        relative right--4.5 cursor-pointer self-center pt-0.5
        @click="ui.createMode === 'raw' ? toggleMode() : null"
      >
        {{ $t('CreateDesign') }}
      </p>
      <p
        :class="ui.createMode === 'raw' ? 'mode raw-active text-white-500 text-shadow-white' : 'mode raw'"
        relative left--4.5 cursor-pointer self-center pt-0.5
        @click="ui.createMode === 'design' ? toggleMode() : null"
      >
        {{ $t('CreateRaw') }}
      </p>
    </div>
    <div
      absolute cursor-pointer
      :right="variant === 'ingredients' ? 2 : 4"
      :h="variant === 'ingredients' ? 12 : 8"
      :w="variant === 'ingredients' ? 12 : 8"
      class="orangeIcon"
      :class="variant === 'ingredients' ? 'i-ph-dots-three-bold' : 'i-ph-question-bold'"
      @click="togglePanel"
    />
  </div>
</template>

<style scoped>
.mode {
  width: 100px;
  height: 34px;
  background-size: 100%;
  background-repeat: no-repeat;
  background-position: center;
  margin: 0 10px;
  z-index: 1;
}

.raw {
  background-image: url('/assets/ModeRaw.svg');
}

.design {
  background-image: url('/assets/ModeDesign.svg');
}

.raw-active {
  background-image: url('/assets/ModeRawActive.svg');
  background-size: 110%;
  z-index: 2;
}

.design-active {
  background-image: url('/assets/ModeDesignActive.svg');
  background-size: 110%;
  z-index: 2;
}
</style>
