<script setup lang="ts">
import { useUIStore } from '~/stores/ui'

const ui = useUIStore()

const currentPage = computed(() => ui.createHeaderInex)

function trueCreate() {
  ui.setCreateHeaderStates(true, currentPage.value - 1)
}

function falseCreate() {
  ui.setCreateHeaderStates(false, currentPage.value - 1)
}
</script>

<template>
  <div flex="~ row" h-20 w-full items-center justify-center gap-10 bg-crimson-50 p-2 px-3 shadow-orange>
    <div v-for="n in 4" :key="n" cursor-pointer :class="[ui.createHeaderStates[n - 1] ? 'done' : 'empty']" @click="ui.setCreateHeaderIndex(n)">
      <div v-if="n === ui.createHeaderInex" class="active" />
    </div>
  </div>
  <div mt-10 flex gap-5>
    <button @click="trueCreate">
      Set True
    </button>
    <button @click="falseCreate">
      Set False
    </button>
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
