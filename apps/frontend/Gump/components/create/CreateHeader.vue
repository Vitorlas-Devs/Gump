<script setup lang="ts">
const ui = useUIStore()
</script>

<template>
  <div flex="~ row" h-20 w-full items-center justify-around bg-crimson-50 p-2 px-3 shadow-orange space-x-xl>
    <div v-for="(state, index) in createTabs" :key="index" cursor-pointer :class="[ui.createHeaderStates[index] ? 'done' : 'empty']" @click="ui.activeCreateTab = state; ui.createHeaderIndex = index">
      <div v-if="state === ui.activeCreateTab" class="active" />
    </div>
  </div>
</template>

<style scoped>
.empty {
  width: 60px;
  height: 20px;

  background: #7A6662;
  box-shadow: 0px 5px 12px -3px rgba(122, 102, 98, 0.6);
  border-radius: 50px;
}

.done {
  width: 60px;
  height: 20px;
  border-radius: 50px;
}

.done:nth-child(1) {
  background: linear-gradient(90deg, #F37927 0%, #D12C5F 460.71%);
  box-shadow: 0px 5px 12px -3px rgba(243, 88, 39, 0.6);
}

.done:nth-child(2) {
  background: linear-gradient(90deg, #F35827 -119.44%, #D12C5F 341.43%);
  box-shadow: 0px 5px 12px -3px rgba(230, 71, 59, 0.6);
}

.done:nth-child(3) {
  background: linear-gradient(90deg, #F35727 -243.89%, #AF0055 218.57%);
  box-shadow: 0px 5px 12px -3px rgba(200, 32, 67, 0.6);
}

.done:nth-child(4) {
  background: linear-gradient(90deg, #F35727 -362.86%, #AF0055 100%);
  box-shadow: 0px 5px 12px -3px rgba(183, 10, 79, 0.6);
}

.active {
  width: 80px;
  height: 40px;

  border: 4px solid rgba(33, 8, 0, 0.9);
  box-shadow: 0px 5px 12px -4px rgba(243, 88, 39, 0.5);
  border-radius: 50px;
  margin-top: -10px;
  margin-left: -10px;
}
</style>
