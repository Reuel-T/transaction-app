<template>
  <div>
    <ul>
      <li v-for="client in sortedClients" :key="client.clientID">
        <p>Client: {{ client.name }} {{ client.surname }}</p>
        <p>Balance: {{ client.clientBalance.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' }) }}</p>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { computed, type PropType } from 'vue';
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


const sortedClients = computed(() => {
  if (props.sortField === 'none' || props.sortOrder === 'none') {
    return [...props.clients];
  } else if (props.sortOrder === 'asc') {
    //any types applied here to make TS shut up
    return [...props.clients].sort((c1: any, c2: any) => {
      return c1[props.sortField] > c2[props.sortField] ? 1 : -1;
    })
  } else if (props.sortOrder === 'desc') {
    return [...props.clients].sort((c1: any, c2: any) => {
      return c1[props.sortField] < c2[props.sortField] ? 1 : -1;
    }) 
  }
  return [...props.clients];
})

</script>

<style scoped lang="scss">

</style>