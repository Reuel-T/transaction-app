<template>
  <div>
    <ul>
      <li v-for="client in props.clients" :key="client.clientID">
        <p>Client: {{ client.name }} {{ client.surname }}</p>
        <p>Balance: {{ client.clientBalance.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' }) }}</p>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';
import type { Client } from '@/models/Client';
import type { ClientSortType } from '@/types/ClientSortTypes';
import type { SortOrder } from '@/types/SortOrder';

const props = defineProps({
  clients: {
    required: true,
    type: Array as PropType<Client[]>
  },
  sortField: {
    default: 'none',
    type: String as PropType<ClientSortType>
  },
  sortOrder: {
    default: 'none',
    type: String as PropType<SortOrder>
  }
})


//onMounted(async () => await fetchClients());

</script>

<style scoped lang="scss">

</style>