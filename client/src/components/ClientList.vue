<template>
  <div>
    <ul>
      <li
        v-for="client in sortedClients"
        :key="client.clientID"
      >
        <ClientData :client="client" :show-view-link="true" />
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import { computed, type PropType } from 'vue'
  import ClientData from './ClientData.vue'
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { ClientSortType } from '@/types/ClientSortTypes'
  import type { SortOrder } from '@/types/SortOrder'

  const props = defineProps({
    clients: {
      required: true,
      type: Array as PropType<ClientDTO[]>
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
      return [...props.clients]
    } else if (props.sortOrder === 'asc') {
      //any types applied here to make TS shut up
      return [...props.clients].sort((c1: any, c2: any) => {
        return c1[props.sortField] > c2[props.sortField] ? 1 : -1
      })
    } else if (props.sortOrder === 'desc') {
      return [...props.clients].sort((c1: any, c2: any) => {
        return c1[props.sortField] < c2[props.sortField] ? 1 : -1
      })
    }
    return [...props.clients]
  })
</script>

<style scoped lang="scss">
  ul {
    list-style-type: none;
  }
</style>
@/models/ClientDTO
