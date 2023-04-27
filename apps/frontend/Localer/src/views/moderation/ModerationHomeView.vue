<script setup lang="ts"></script>

<template>
  <main flex="~ col" w="full" h="full" p="2 md:6" pl="4 md:10" mt="2" mr="-5">
    <custom-scrollbar :auto-expand="false" h="screen" w="full" pb="25">
      <h1 text="3xl" font="bold" mb="4">Moderation</h1>
	  <p>This is the moderation interface. You can modify or delete recipes and users. There's full CRUD access to the partners and adverts too.</p>
    </custom-scrollbar>
    <div bg="green" display="none" />
  </main>
</template>
