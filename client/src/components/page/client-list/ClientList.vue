<template>
  <!-- Card Title + Sorting Fields -->
  <div class="title-row">
    <div
      style="flex: 2"
      class="title-item"
    >
      <h2>Clients</h2>
    </div>

    <div
      style="flex: 3"
      class="title-item"
    >
      <label
        style="width: 50%"
        for="searchField"
        >Search Clients</label
      >
      <input
        id="searchField"
        type="text"
        v-model.trim="searchField"
      />
    </div>

    <div
      style="flex: 2"
      class="title-item"
    >
      <label
        style="flex: 1; text-align: right"
        for="sortField"
        >Sort By:</label
      >
      <select
        id="sortField"
        v-model="sortField"
        style="margin-left: 0.5rem"
      >
        <option value="clientID">Default</option>
        <option value="name">Name</option>
        <option value="surname">Surname</option>
        <option value="clientBalance">Balance</option>
      </select>
      <select
        id="sortOrder"
        v-model="sortOrder"
        style="margin-left: 0.5rem"
      >
        <option value="asc">ASC</option>
        <option value="desc">DESC</option>
      </select>
    </div>
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

  <transition-group
    tag="div"
    class="scroll"
    name="list"
    appear
  >
    <ClientData
      v-for="client in sortedClients"
      :key="client.clientID"
      :client="client"
    />
  </transition-group>
</template>

<script setup lang="ts">
  import { computed, ref, type PropType } from 'vue'
  import ClientData from '@/components/page/client-list/ClientData.vue'
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { ClientSortType } from '@/types/ClientSortTypes'
  import type { SortOrder } from '@/types/SortOrder'

  const sortField = ref<ClientSortType>('clientID')
  const sortOrder = ref<SortOrder>('asc')

  const searchField = ref<string>('')

  const props = defineProps({
    clients: {
      required: true,
      type: Array as PropType<ClientDTO[]>
    }
  })

  const sortedClients = computed(() => {
    const filtered = [...props.clients].filter((client) => {
      return `${client.name} ${client.surname}`
        .toLowerCase()
        .includes(searchField.value.toLowerCase())
    })

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
    position: relative;

    scrollbar-width: thin;
  }

  .title-row {
    width: 100%;
    align-items: center;
    display: flex;
    flex-direction: row;
    h2 {
      margin: 0;
      flex-grow: 1;
    }
    padding: 0 1rem;
    .title-item {
      align-items: center;
      display: flex;
      flex-direction: row;
    }
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

  /* LIST TRANSITIONS */
  .list-enter-from {
    opacity: 0;
    transform: scale(0.6);
  }

  .list-enter-to {
    opacity: 1;
    transform: scale(1);
  }

  .list-enter-active {
    transition: all 0.4s ease;
  }

  .list-leave-from {
    opacity: 1;
    transform: scale(1);
  }

  .list-leave-to {
    opacity: 0;
    transform: scale(0.6);
  }

  .list-leave-active {
    transition: all 0.4s ease;
    position: absolute;
    /* 
      Used to make the items slide when others are moved.
      Just remember to make the parent position relative 
    */
  }

  /* animation for items moving */
  /* 
    Internally vue will handle moving the items, 
    this just sets the timing and easing
  */
  .list-move {
    transition: all 0.3s ease;
  }
</style>
