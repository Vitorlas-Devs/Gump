<script setup lang="ts">
const ui = useUIStore()
</script>

<template>
  <div flex="~ row" h-20 w-full items-center justify-around gap-10 bg-crimson-50 p-2 px-3 shadow-orange>
    <div v-for="(state, index) in ui.createHeaderStates" :key="index" cursor-pointer :class="[ui.createHeaderStates[index] ? 'done' : 'empty']" @click="ui.createHeaderIndex = index">
      <div v-if="index === ui.createHeaderIndex" class="active" />
    </div>
  </div>
</template>

<style scoped>
.empty {
  width: 70px;
  height: 20px;

  background: #7A6662;
  box-shadow: 0px 5px 12px -3px rgba(122, 102, 98, 0.6);
  border-radius: 50px;
}

.done {
  width: 70px;
  height: 20px;

  background: linear-gradient(90deg, #F37927 0%, #D12C5F 460.71%);
  box-shadow: 0px 5px 12px -3px rgba(243, 88, 39, 0.6);
  border-radius: 50px;
}

.active {
  width: 90px;
  height: 40px;

  border: 4px solid rgba(33, 8, 0, 0.9);
  box-shadow: 0px 5px 12px -4px rgba(243, 88, 39, 0.5);
  border-radius: 50px;
  margin-top: -10px;
  margin-left: -10px;
}
</style>
