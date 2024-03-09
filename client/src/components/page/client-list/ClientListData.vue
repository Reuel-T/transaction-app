<template>
  <div class="client">
    <div class="client-field">
      <h3 class="kanit-light">{{ props.client.name }}</h3>
    </div>
    <div class="client-field">
      <h3 class="kanit-light">{{ props.client.surname }}</h3>
    </div>
    <div class="client-field">
      <h3 class="kanit-light">
        {{
          props.client.clientBalance.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' })
        }}
      </h3>
    </div>
    <div
      class="client-end"
      v-if="props.showViewLink"
    >
      <RouterLink
        role="button"
        :to="`/client/${props.client.clientID}`"
      >
        <button class="btn pill">View Transactions</button>
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { ClientDTO } from '@/models/ClientDTO'
  import type { PropType } from 'vue'

  const props = defineProps({
    client: {
      required: true,
      type: Object as PropType<ClientDTO>
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

    .client-field {
      border-right: 2px solid rgba(255, 255, 255, 0.25);

      width: calc((100% - 12rem) / 3);

      align-self: center;
      justify-self: center;

      h3 {
        margin: 0.25rem 1rem;
        padding: 0;
        text-align: start;
      }
    }

    .client-end {
      align-self: center;
      justify-self: center;
      min-width: 12rem;

      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
</style>
