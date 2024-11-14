<template>
  Facility id : 
  {{facilityId}}
  Selected : 
  {{selId}}
  <RoomComponent @selectChart="selectChart" :facilityId="facilityId"/>

  <q-drawer
    v-model="drawerRight"
    class="bg-light-100"
    side="right"
    :width="300"
    :breakpoint="500"
  >
    <ClientBookingForm :facilityId="facilityId" :selected-id="selId" />
  </q-drawer>
  
</template>

<script lang="ts" setup>
import RoomComponent from '@/components/Room.vue';
import ClientBookingForm from '@/components/ClientBookingForm.vue';
import { ChartItem } from '@/types/draggable-panel';
import { onMounted, Ref, ref } from 'vue';
import { QDrawer } from 'quasar';
import { useRoute } from 'vue-router';
import { Facility } from '@/model/facility';
import { useFacilitiesStore } from '@/stores/facilityStore';
import { storeToRefs } from 'pinia';

const drawerRight = ref(true);

const facilitiesStore = useFacilitiesStore();
const facilityThis: Ref<Facility> = ref<Facility>({})
const { facility } = storeToRefs(facilitiesStore);

const facilityId: Ref<string> = ref<string>('');

onMounted(async () => {
  facilityId.value = String(useRoute().params.id);
  await facilitiesStore.loadFicilityById(facilityId.value);
  facilityThis.value = facility.value!
});

const selId= ref('');
const selectChart = (chart: ChartItem) => {
  selId.value = chart.id!;
  return selId.value;
};

</script>

<style scoped>
.seats{
  width: 100%;
  height: auto;
  opacity: 0.85;
}
</style>
