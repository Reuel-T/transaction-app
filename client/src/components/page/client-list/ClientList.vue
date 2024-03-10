<template>
  <!-- Card Title + Sorting Fields -->
  <div class="title-row">
    <h2>Clients</h2>
    <label for="searchField">Search Clients</label>
    <input id="searchField" type="text" v-model.trim="searchField"/>
    <label for="sortField">Sort By:</label>
    <select
      id="sortField"
      v-model="sortField"
    >
      <option value="clientID">Default</option>
      <option value="name">Name</option>
      <option value="surname">Surname</option>
      <option value="clientBalance">Balance</option>
    </select>
    <select v-model="sortOrder">
      <option value="asc">ASC</option>
      <option value="desc">DESC</option>
    </select>
  </div>

  <div class="heading-row">
    <div class="heading-row-item">
      <h3>Name</h3>
    </div>
    <div class="heading-row-item">
      <h3>Surname</h3>
    </div>
    <div class="heading-row-item">
      <h3>Balance</h3>
    </div>
    <div class="heading-row-end"></div>
  </div>

  <div class="scroll">
    <ClientData
      v-for="client in sortedClients"
      :key="client.clientID"
      :client="client"
    />
  </div>
</template>

<script setup lang="ts">
  import { computed, ref, type PropType } from 'vue'
  import ClientData from '@/components/page/client-list/ClientData.vue';
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { ClientSortType } from '@/types/ClientSortTypes'
  import type { SortOrder } from '@/types/SortOrder'

  const sortField = ref<ClientSortType>('clientID')
  const sortOrder = ref<SortOrder>('asc')

  const searchField = ref<string>('');
  
  const props = defineProps({
    clients: {
      required: true,
      type: Array as PropType<ClientDTO[]>
    }
  })

  const sortedClients = computed(() => {
    const filtered = [...props.clients].filter((client) => {
      return `${client.name} ${client.surname}`.toLowerCase().includes(searchField.value.toLowerCase());
    });

    if (sortOrder.value === 'asc') {
      return filtered.sort((c1: ClientDTO, c2: ClientDTO) => {
        return c1[sortField.value] > c2[sortField.value] ? 1 : -1
      })
    } else {
      return filtered.sort((c1: ClientDTO, c2: ClientDTO) => {
        return c1[sortField.value] < c2[sortField.value] ? 1 : -1
      })
    }
  })
</script>

<style scoped lang="scss">
.scroll {
    height: 100%;
    overflow-y: auto;

    scrollbar-width: thin;
  }

.title-row {
    align-items: center;
    display: flex;
    flex-direction: row;
    h2 {
      margin: 0;
      flex-grow: 1;
    }
    padding: 0 1rem;
  }

  .heading-row {
    display: flex;
    flex-direction: row;

    .heading-row-item {
      width: 30%;
      h3 {
        text-align: center;
      }
    }

    .heading-row-end {
      width: 10%;
    }
  }
</style>
