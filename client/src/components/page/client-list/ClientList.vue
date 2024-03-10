<template>
  <div>
    <div class="table-headings">
      <div
        @click="
          () => {
            setDefaultSortOrderForField('name')
          }
        "
        class="table-field"
      >
        <h3 :class="sortField === 'name' && sortOrder !== 'none' ? '' : 'kanit-regular'">Name</h3>
      </div>
      <div
        @click="
          () => {
            setDefaultSortOrderForField('surname')
          }
        "
        class="table-field"
      >
        <h3 :class="sortField === 'surname' && sortOrder !== 'none' ? '' : 'kanit-regular'">
          Surname
        </h3>
      </div>
      <div
        @click="
          () => {
            setDefaultSortOrderForField('clientBalance')
          }
        "
        class="table-field"
      >
        <h3 :class="sortField === 'clientBalance' && sortOrder !== 'none' ? '' : 'kanit-regular'">
          Balance
        </h3>
      </div>
      <div class="table-end">
        <select
          v-show="sortField !== 'none'"
          v-model="sortOrder"
        >
          <option value="none">Unsorted</option>
          <option value="asc">ASC</option>
          <option value="desc">DESC</option>
        </select>
      </div>
    </div>

    <ul>
      <li
        v-for="client in sortedClients"
        :key="client.clientID"
      >
        <ClientListData
          :client="client"
          :show-view-link="true"
        />
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import { computed, ref, type PropType } from 'vue'
  import ClientListData from '@/components/page/client-list/ClientListData.vue'
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { ClientSortType } from '@/types/ClientSortTypes'
  import type { SortOrder } from '@/types/SortOrder'

  const sortField = ref<ClientSortType>('none')
  const sortOrder = ref<SortOrder>('none')

  function setDefaultSortOrderForField(field: ClientSortType) {
    switch (field) {
      case 'clientBalance':
        if (sortField.value === 'clientBalance') {
          resetOrder()
        } else {
          sortField.value = 'clientBalance'
          sortOrder.value = 'desc'
        }

        break
      case 'name':
        if (sortField.value === 'name') {
          resetOrder()
        } else {
          sortField.value = 'name'
          sortOrder.value = 'asc'
        }
        break
      case 'surname':
        if (sortField.value === 'surname') {
          resetOrder()
        } else {
          sortField.value = 'surname'
          sortOrder.value = 'asc'
        }

        break
      default:
        sortField.value = 'none'
        sortOrder.value = 'none'
        break
    }
  }

  function resetOrder() {
    sortField.value = 'none'
    sortOrder.value = 'none'
  }

  const props = defineProps({
    clients: {
      required: true,
      type: Array as PropType<ClientDTO[]>
    }
  })

  const sortedClients = computed(() => {
    if (sortField.value === 'none' || sortOrder.value === 'none') {
      return [...props.clients]
    } else if (sortOrder.value === 'asc') {
      //any types applied here to make TS shut up
      return [...props.clients].sort((c1: any, c2: any) => {
        return c1[sortField.value] > c2[sortField.value] ? 1 : -1
      })
    } else if (sortOrder.value === 'desc') {
      return [...props.clients].sort((c1: any, c2: any) => {
        return c1[sortField.value] < c2[sortField.value] ? 1 : -1
      })
    }
    return [...props.clients]
  })
</script>

<style scoped lang="scss">
  .table-headings {
    display: flex;
    flex-direction: row;

    .table-field {
      border-right: 2px solid rgba(255, 255, 255, 0.25);

      width: calc((100% - 12rem) / 3);

      align-self: center;
      justify-self: center;
      cursor: pointer;

      h3 {
        margin: 0;
        padding: 0;
        text-align: center;
      }
    }

    .table-end {
      align-self: center;
      justify-self: center;
      min-width: 12rem;

      h3 {
        margin: 0;
        padding: 0;
        text-align: center;
      }
    }
  }

  // Style the select element
  select {
    appearance: none;
    border: 2px solid #b4b4b4;
    border-radius: 20px;
    background-color: transparent;
    color: #b4b4b4;
    cursor: pointer;
    text-align: center;
    transition:
      border-color 0.3s,
      background-color 0.3s,
      color 0.3s;

    &:hover {
      border-color: white;
      color: white;
    }

    // Style the options within the select
    option {
      padding: 10px;
      font-size: 14px;
      background-color: transparent;
      color: #333;
    }

    // Style the options on hover
    option:hover {
      background-color: #e0e0e0;
    }

    // Style the selected option
    & option:checked {
      background-color: #3498db;
      color: #fff;
    }
  }

  ul {
    margin: 0;
    padding: 0;
    list-style-type: none;

    li {
      margin: 0;
      padding: 0;
    }
  }
</style>
