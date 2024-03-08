<template>
  <div>
    <h1>Home View!</h1>
    <div v-if="isLoading">
      <p>Loading...</p>
    </div>
    <div v-else-if="isError">
      <p>There was an Error</p>
    </div>
    <div v-if="!isLoading && !isError">
      <button @click="sortFieldClicked">{{ sortField }}</button>
      <button @click="sortOrderClicked">{{ sortOrder }}</button>
      <ClientList :clients="data" :sort-field="sortField" :sort-order="sortOrder" />
    </div>
  </div>
</template>

<script setup lang="ts">
import ClientList from '@/components/ClientList.vue';
import { useClients } from '@/shared/useClients';
import type { ClientSortType } from '@/types/ClientSortTypes';
import type { SortOrder } from '@/types/SortOrder';
import { ref } from 'vue';

const sortField = ref<ClientSortType>('none');
const sortOrder = ref<SortOrder>('none');

const { data, isError, isLoading } = useClients();

function sortFieldClicked() {
  switch (sortField.value) {
    case 'none':
      sortField.value = 'name';
      break;
    case 'name':
      sortField.value = 'surname';
      break;
    case 'surname':
      sortField.value = 'clientBalance';
      break;
    default:
      sortField.value = 'none';
      break;
  }
}

function sortOrderClicked() {
  switch (sortOrder.value) {
    case 'none':
      sortOrder.value = 'asc';
      break;
    case 'asc':
      sortOrder.value = 'desc';
      break;
    default:
      sortOrder.value = 'none';
      break;
  }
}

</script>

<style scoped lang="scss">

</style>