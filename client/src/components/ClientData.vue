<template>
  <div class="client">
    <div class="info">
      <p>{{ props.client.name }}</p>
    </div>
    <div class="info">
      <p>{{ props.client.surname }}</p>
    </div>
    <div class="info">
      <p>
        {{
          props.client.clientBalance.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' })
        }}
      </p>
    </div>
    <div
      v-if="props.showViewLink"
      class="info"
    >
      <RouterLink :to="`/client/${props.client.clientID}`">View</RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { Client } from '@/models/Client'
  import type { PropType } from 'vue'

  const props = defineProps({
    client: {
      required: true,
      type: Object as PropType<Client>
    },
    showViewLink: {
      required: false,
      type: Boolean,
      default: false
    }
  })
</script>

<style lang="scss" scoped>
  .client {
    display: flex;
    flex-direction: row;

    .info {
      width: 10rem;
    }
  }
</style>
@/models/ClientDTO
