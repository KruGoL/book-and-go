<template>
  Link: 
  http://127.0.0.1:5173/client/{{facilityId}}
  <AdminBookingTable
    :facilityId="facilityId"
    :tableFilter="filter"
  />
  <RoomComponent @selectChart="selectChart" :facilityId="facilityId"/>
</template>

<script lang="ts" setup>
import AdminBookingTable from '@/components/AdminBookingTable.vue';
import RoomComponent from '@/components/Room.vue';

import { Facility } from '@/model/facility';
import { useFacilitiesStore } from '@/stores/facilityStore';
import { ChartItem } from '@/types/draggable-panel';
import { storeToRefs } from 'pinia';
import { ref, Ref, onMounted } from 'vue';

const facilitiesStore = useFacilitiesStore();
let facilities = ref<Facility[]>([]);
const facility: Ref<Facility> | undefined = ref<Facility>({});
const { Facilities } = storeToRefs(facilitiesStore);

const facilityId: Ref<string> = ref<string>('');
onMounted(async () => {
  await facilitiesStore.loadAcc();
  facilities = Facilities;
  facilityId.value = facilities.value[0].id!;
  facility.value = facilitiesStore.getAccFacilityById(facilityId.value)!;
});

const filter = ref('');
const selectChart = (chart: ChartItem) => {
  filter.value = chart.id!;
  return filter.value;
};
</script>
