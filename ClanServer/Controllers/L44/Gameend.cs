using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;

using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers.L44
{
    [ApiController, Route("L44")]
    public class GameendController : ControllerBase
    {
        /*
<call model="L44:J:E:A:2018070901" srcid="01201000009FDCEA1A6C" tag="dRstALRF">
  <gameend method="regist">
    <retry __type="s32">0</retry>
    <pcbinfo client_data_version="0" />
    <data>
      <info>
        <time_gametop __type="u64">1559483385853</time_gametop>
        <time_gameend __type="u64">1559483977121</time_gameend>
        <play_time __type="s32" played_tunes="3" purchased_tunes="3">573</play_time>
        <cabid __type="s32">1</cabid>
        <payment __type="s8">0</payment>
        <mode __type="s8">1</mode>
        <shopname __type="str">Oromit Arcade</shopname>
        <areaname __type="str">JP-14</areaname>
        <customer_code __type="str"></customer_code>
        <system_id __type="str">01201000009FDCEA1A6C</system_id>
        <location_id __type="str">8C4E59A1</location_id>
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <payment_list>
          <payment>
            <type __type="str">COIN</type>
            <event __type="str">game.s.normal</event>
            <amount __type="s32">0</amount>
            <count __type="s32">1</count>
          </payment>
          <payment>
            <type __type="str">COIN</type>
            <event __type="str">game.s.free.normal</event>
            <amount __type="s32">0</amount>
            <count __type="s32">1</count>
          </payment>
        </payment_list>
      </info>
      <result count="3">
        <tune id="1">
          <music __type="s32">70000183</music>
          <roomid __type="s64">1832734206069547163</roomid>
          <timestamp __type="s64">1559483541000</timestamp>
          <player rank="1">
            <score __type="s32" clear="3" combo="239" diff="0" seq="1">917287</score>
            <marker __type="s8">4</marker>
            <theme __type="s8">7</theme>
            <category __type="s8">24</category>
            <is_recommend __type="bool">0</is_recommend>
            <elapsed_time __type="s32">0</elapsed_time>
            <is_timeout __type="bool">0</is_timeout>
            <is_hard_mode __type="bool">0</is_hard_mode>
            <is_hazard_end __type="bool">0</is_hazard_end>
            <is_consumed_ex_option __type="bool">0</is_consumed_ex_option>
            <nr_perfect __type="s16">372</nr_perfect>
            <nr_great __type="s16">102</nr_great>
            <nr_good __type="s16">13</nr_good>
            <nr_poor __type="s16">0</nr_poor>
            <nr_miss __type="s16">7</nr_miss>
            <is_first_play __type="bool">1</is_first_play>
            <is_first_clear __type="bool">1</is_first_clear>
            <is_first_fullcombo __type="bool">0</is_first_fullcombo>
            <is_first_excellent __type="bool">0</is_first_excellent>
            <is_first_nogray __type="bool">0</is_first_nogray>
            <is_first_all_yellow __type="bool">0</is_first_all_yellow>
            <best_score __type="s32">917287</best_score>
            <best_clear __type="s8">3</best_clear>
            <play_cnt __type="s32">1</play_cnt>
            <clear_cnt __type="s32">1</clear_cnt>
            <fc_cnt __type="s32">0</fc_cnt>
            <ex_cnt __type="s32">0</ex_cnt>
            <mbar __type="u8" __count="30">239 191 234 175 190 171 175 186 255 191 190 254 255 255 255 255 155 169 191 233 191 251 127 187 218 251 171 170 250 255</mbar>
            <play_mbar __type="u8" __count="30">239 191 234 175 190 171 175 186 255 191 190 254 255 255 255 255 155 169 191 233 191 251 127 187 218 251 171 170 250 255</play_mbar>
          </player>
          <meeting>
            <single count="0" />
          </meeting>
        </tune>
        <tune id="2">
          <music __type="s32">60000043</music>
          <roomid __type="s64">-8966206469043710946</roomid>
          <timestamp __type="s64">1559483685000</timestamp>
          <player rank="1">
            <score __type="s32" clear="19" combo="357" diff="0" seq="1">939690</score>
            <marker __type="s8">4</marker>
            <theme __type="s8">7</theme>
            <category __type="s8">24</category>
            <is_recommend __type="bool">0</is_recommend>
            <elapsed_time __type="s32">0</elapsed_time>
            <is_timeout __type="bool">0</is_timeout>
            <is_hard_mode __type="bool">0</is_hard_mode>
            <is_hazard_end __type="bool">0</is_hazard_end>
            <is_consumed_ex_option __type="bool">0</is_consumed_ex_option>
            <nr_perfect __type="s16">384</nr_perfect>
            <nr_great __type="s16">95</nr_great>
            <nr_good __type="s16">5</nr_good>
            <nr_poor __type="s16">0</nr_poor>
            <nr_miss __type="s16">1</nr_miss>
            <is_first_play __type="bool">1</is_first_play>
            <is_first_clear __type="bool">1</is_first_clear>
            <is_first_fullcombo __type="bool">0</is_first_fullcombo>
            <is_first_excellent __type="bool">0</is_first_excellent>
            <is_first_nogray __type="bool">0</is_first_nogray>
            <is_first_all_yellow __type="bool">0</is_first_all_yellow>
            <best_score __type="s32">939690</best_score>
            <best_clear __type="s8">19</best_clear>
            <play_cnt __type="s32">1</play_cnt>
            <clear_cnt __type="s32">1</clear_cnt>
            <fc_cnt __type="s32">0</fc_cnt>
            <ex_cnt __type="s32">0</ex_cnt>
            <mbar __type="u8" __count="30">251 255 255 175 186 238 239 250 251 255 250 171 187 190 238 171 175 187 190 235 250 122 174 235 175 190 175 238 250 255</mbar>
            <play_mbar __type="u8" __count="30">251 255 255 175 186 238 239 250 251 255 250 171 187 190 238 171 175 187 190 235 250 122 174 235 175 190 175 238 250 255</play_mbar>
          </player>
          <meeting>
            <single count="0" />
          </meeting>
        </tune>
        <tune id="3">
          <music __type="s32">60001001</music>
          <roomid __type="s64">624634556024109711</roomid>
          <timestamp __type="s64">1559483966000</timestamp>
          <player rank="1">
            <score __type="s32" clear="3" combo="141" diff="0" seq="1">915413</score>
            <marker __type="s8">4</marker>
            <theme __type="s8">7</theme>
            <category __type="s8">24</category>
            <is_recommend __type="bool">0</is_recommend>
            <elapsed_time __type="s32">0</elapsed_time>
            <is_timeout __type="bool">0</is_timeout>
            <is_hard_mode __type="bool">0</is_hard_mode>
            <is_hazard_end __type="bool">0</is_hazard_end>
            <is_consumed_ex_option __type="bool">0</is_consumed_ex_option>
            <nr_perfect __type="s16">304</nr_perfect>
            <nr_great __type="s16">76</nr_great>
            <nr_good __type="s16">10</nr_good>
            <nr_poor __type="s16">3</nr_poor>
            <nr_miss __type="s16">6</nr_miss>
            <is_first_play __type="bool">1</is_first_play>
            <is_first_clear __type="bool">1</is_first_clear>
            <is_first_fullcombo __type="bool">0</is_first_fullcombo>
            <is_first_excellent __type="bool">0</is_first_excellent>
            <is_first_nogray __type="bool">0</is_first_nogray>
            <is_first_all_yellow __type="bool">0</is_first_all_yellow>
            <best_score __type="s32">915413</best_score>
            <best_clear __type="s8">3</best_clear>
            <play_cnt __type="s32">1</play_cnt>
            <clear_cnt __type="s32">1</clear_cnt>
            <fc_cnt __type="s32">0</fc_cnt>
            <ex_cnt __type="s32">0</ex_cnt>
            <mbar __type="u8" __count="30">239 254 239 214 229 253 122 186 182 235 239 191 190 251 186 174 122 234 254 255 251 190 235 251 251 255 187 174 255 255</mbar>
            <play_mbar __type="u8" __count="30">239 254 239 214 229 253 122 186 182 235 239 191 190 251 186 174 122 234 254 255 251 190 235 251 251 255 187 174 255 255</play_mbar>
          </player>
          <meeting>
            <single count="0" />
          </meeting>
        </tune>
      </result>
      <player>
        <session_id __type="s32">1</session_id>
        <refid __type="str">DD389C3FFB6F47BA</refid>
        <jid __type="s32">612645048</jid>
        <name __type="str">CLANTEST</name>
        <event_flag __type="u64">2</event_flag>
        <continue_count __type="s32">0</continue_count>
        <info>
          <tune_cnt __type="s32">994</tune_cnt>
          <clear_cnt __type="s32">949</clear_cnt>
          <fc_cnt __type="s32">40</fc_cnt>
          <ex_cnt __type="s32">0</ex_cnt>
          <match_cnt __type="s32">0</match_cnt>
          <beat_cnt __type="s32">0</beat_cnt>
          <save_cnt __type="s32">0</save_cnt>
          <saved_cnt __type="s32">45</saved_cnt>
          <total_best_score __type="s64">2772390</total_best_score>
          <bonus_tune_points __type="s32">740</bonus_tune_points>
          <is_bonus_tune_played __type="bool">1</is_bonus_tune_played>
          <clear_max_level __type="u8">7</clear_max_level>
          <fc_max_level __type="u8">0</fc_max_level>
          <ex_max_level __type="u8">0</ex_max_level>
          <total_music_num __type="s32">844</total_music_num>
          <default_music_num __type="s32">300</default_music_num>
          <unlock_music_num __type="s32">844</unlock_music_num>
          <last_play_time __type="s64">0</last_play_time>
          <mynews_cnt __type="s32">0</mynews_cnt>
        </info>
        <last>
          <expert_option __type="s8">0</expert_option>
          <sort __type="s8">3</sort>
          <category __type="s8">24</category>
          <settings>
            <marker __type="s8">3</marker>
            <theme __type="s8">6</theme>
            <title __type="s16">0</title>
            <parts __type="s16">0</parts>
            <rank_sort __type="s8">1</rank_sort>
            <combo_disp __type="s8">1</combo_disp>
            <emblem __type="s16" __count="5">0 823 0 0 0</emblem>
            <matching __type="s8">0</matching>
            <hard __type="s8">0</hard>
            <hazard __type="s8">0</hazard>
          </settings>
        </last>
        <item>
          <music_list __type="s32" __count="64">-2013265951 -102760493 1711275733 -1579088899 -108536 -227069 -33554401 16383 0 -1377473 -402653185 -2097153 -1231036417 -786433 -444727297 -1 980541439 -33357824 1077928957 133988323 1075838048 -32706 -234907777 -196609 33138687 -2097152 -907557381 -2 -134217841 -34734081 -524293 -1641628417 -1 -1 -2177 -7532097 -3 264241151 2080768 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</music_list>
          <secret_list __type="s32" __count="64">-2013265951 -102760493 1711275733 -1579088899 -108536 -227069 -33554401 16383 0 -1377473 -402653185 -2097153 -1231036417 -786433 -444727297 -1 980541439 -33357824 1077928957 133988323 1075838048 -32706 -234907777 -196609 33138687 -2097152 -907557381 -2 -134217841 -34734081 -524293 -1641628417 -1 -1 -2177 -7532097 -3 264241151 2080768 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</secret_list>
          <theme_list __type="s32" __count="16">2047 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</theme_list>
          <marker_list __type="s32" __count="16">-1 7167 0 0 0 0 0 0 0 0 0 0 0 0 0 0</marker_list>
          <title_list __type="s32" __count="160">1 0 0 0 0 65536 0 0 0 0 0 1024 1048576 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</title_list>
          <parts_list __type="s32" __count="160">1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 -2147483648 0 32 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</parts_list>
          <emblem_list __type="s32" __count="96">1 0 0 0 0 0 0 16 0 0 0 0 0 536870916 0 0 65536 0 4194304 0 0 0 268435456 0 65536 8388624 0 0 0 0 0 0 0 147456 0 0 0 4194312 33554432 205520896 0 0 2080 2 0 512 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</emblem_list>
          <new>
            <secret_list __type="s32" __count="64">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</secret_list>
            <theme_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</theme_list>
            <marker_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</marker_list>
          </new>
          <commu_list __type="s32" __count="16">-1 7167 0 0 0 0 0 0 0 0 0 0 0 0 0 0</commu_list>
        </item>
        <meeting>
          <reward>
            <total __type="s32">0</total>
            <point __type="s32">0</point>
          </reward>
        </meeting>
        <fc_challenge />
        <free_first_play>
          <is_applied __type="bool">0</is_applied>
        </free_first_play>
        <event_info>
          <event />
        </event_info>
        <official_news>
          <news_list />
        </official_news>
        <mynews count="0" />
        <jbox>
          <point __type="s32">800</point>
          <emblem>
            <type __type="u8">0</type>
            <index __type="s16">0</index>
          </emblem>
        </jbox>
        <department>
          <item_list />
        </department>
        <history count="3">
          <tune count="0" log_id="3">
            <timestamp __type="s64">1559483966000</timestamp>
            <release_code __type="str">2018070901</release_code>
            <mode __type="s8">1</mode>
            <player music_id="60001001" rank="1">
              <info>
                <jid __type="s32">612645048</jid>
                <name __type="str">CLANTEST</name>
                <title __type="s16">0</title>
                <parts __type="s16">0</parts>
                <emblem __type="s16" __count="5">0 823 0 0 0</emblem>
                <team_id __type="s32">1</team_id>
              </info>
              <result>
                <shopname __type="str">Oromit Arcade</shopname>
                <areaname __type="str">JP-14</areaname>
                <score __type="s32" clear="3" diff="0" seq="1">915413</score>
                <is_hard_mode __type="bool">0</is_hard_mode>
                <nr_perfect __type="s16">304</nr_perfect>
                <nr_great __type="s16">76</nr_great>
                <nr_good __type="s16">10</nr_good>
                <nr_poor __type="s16">3</nr_poor>
                <nr_miss __type="s16">6</nr_miss>
                <mbar __type="u8" __count="30">239 254 239 214 229 253 122 186 182 235 239 191 190 251 186 174 122 234 254 255 251 190 235 251 251 255 187 174 255 255</mbar>
              </result>
            </player>
          </tune>
          <tune count="0" log_id="2">
            <timestamp __type="s64">1559483685000</timestamp>
            <release_code __type="str">2018070901</release_code>
            <mode __type="s8">1</mode>
            <player music_id="60000043" rank="1">
              <info>
                <jid __type="s32">612645048</jid>
                <name __type="str">CLANTEST</name>
                <title __type="s16">0</title>
                <parts __type="s16">0</parts>
                <emblem __type="s16" __count="5">0 823 0 0 0</emblem>
                <team_id __type="s32">1</team_id>
              </info>
              <result>
                <shopname __type="str">Oromit Arcade</shopname>
                <areaname __type="str">JP-14</areaname>
                <score __type="s32" clear="19" diff="0" seq="1">939690</score>
                <is_hard_mode __type="bool">0</is_hard_mode>
                <nr_perfect __type="s16">384</nr_perfect>
                <nr_great __type="s16">95</nr_great>
                <nr_good __type="s16">5</nr_good>
                <nr_poor __type="s16">0</nr_poor>
                <nr_miss __type="s16">1</nr_miss>
                <mbar __type="u8" __count="30">251 255 255 175 186 238 239 250 251 255 250 171 187 190 238 171 175 187 190 235 250 122 174 235 175 190 175 238 250 255</mbar>
              </result>
            </player>
          </tune>
          <tune count="0" log_id="1">
            <timestamp __type="s64">1559483541000</timestamp>
            <release_code __type="str">2018070901</release_code>
            <mode __type="s8">1</mode>
            <player music_id="70000183" rank="1">
              <info>
                <jid __type="s32">612645048</jid>
                <name __type="str">CLANTEST</name>
                <title __type="s16">0</title>
                <parts __type="s16">0</parts>
                <emblem __type="s16" __count="5">0 823 0 0 0</emblem>
                <team_id __type="s32">1</team_id>
              </info>
              <result>
                <shopname __type="str">Oromit Arcade</shopname>
                <areaname __type="str">JP-14</areaname>
                <score __type="s32" clear="3" diff="0" seq="1">917287</score>
                <is_hard_mode __type="bool">0</is_hard_mode>
                <nr_perfect __type="s16">372</nr_perfect>
                <nr_great __type="s16">102</nr_great>
                <nr_good __type="s16">13</nr_good>
                <nr_poor __type="s16">0</nr_poor>
                <nr_miss __type="s16">7</nr_miss>
                <mbar __type="u8" __count="30">239 191 234 175 190 171 175 186 255 191 190 254 255 255 255 255 155 169 191 233 191 251 127 187 218 251 171 170 250 255</mbar>
              </result>
            </player>
          </tune>
        </history>
        <eapass>
          <cardtype __type="str">2</cardtype>
          <card_no __type="str">BBL31FY455J6TU2Z</card_no>
          <is_completely_new __type="bool">0</is_completely_new>
          <is_paseli_available __type="bool">1</is_paseli_available>
        </eapass>
        <navi>
          <flag __type="u64">122</flag>
        </navi>
        <gift_list />
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <jubility param="68">
          <target_music_list>
            <target_music>
              <music_id __type="s32">60000043</music_id>
              <seq __type="s8">1</seq>
              <score __type="s32">939690</score>
              <value __type="s32">704</value>
              <is_hard_mode __type="bool">0</is_hard_mode>
            </target_music>
            <target_music>
              <music_id __type="s32">60001001</music_id>
              <seq __type="s8">1</seq>
              <score __type="s32">915413</score>
              <value __type="s32">675</value>
              <is_hard_mode __type="bool">0</is_hard_mode>
            </target_music>
            <target_music>
              <music_id __type="s32">70000183</music_id>
              <seq __type="s8">1</seq>
              <score __type="s32">917287</score>
              <value __type="s32">664</value>
              <is_hard_mode __type="bool">0</is_hard_mode>
            </target_music>
          </target_music_list>
        </jubility>
        <team id="1">
          <section __type="s32">4</section>
          <street __type="s32">6</street>
          <house_number_1 __type="s32">67</house_number_1>
          <house_number_2 __type="s32">1</house_number_2>
        </team>
        <server />
        <clan_course_list>
          <clan_course id="50">
            <status __type="s8">1</status>
          </clan_course>
          <category_list>
            <category id="1">
              <is_display __type="bool">1</is_display>
            </category>
            <category id="2">
              <is_display __type="bool">1</is_display>
            </category>
            <category id="3">
              <is_display __type="bool">1</is_display>
            </category>
            <category id="4">
              <is_display __type="bool">1</is_display>
            </category>
            <category id="5">
              <is_display __type="bool">1</is_display>
            </category>
            <category id="6">
              <is_display __type="bool">1</is_display>
            </category>
          </category_list>
        </clan_course_list>
        <drop_list>
          <drop id="1">
            <exp __type="s32">30</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="2">
            <exp __type="s32">30</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="3">
            <exp __type="s32">30</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="4">
            <exp __type="s32">30</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="5">
            <exp __type="s32">30</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="6">
            <exp __type="s32">0</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="7">
            <exp __type="s32">0</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="8">
            <exp __type="s32">0</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="9">
            <exp __type="s32">0</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
          <drop id="10">
            <exp __type="s32">0</exp>
            <flag __type="s32">0</flag>
            <item_list>
              <item id="1">
                <num __type="s32">0</num>
              </item>
              <item id="2">
                <num __type="s32">0</num>
              </item>
              <item id="3">
                <num __type="s32">0</num>
              </item>
              <item id="4">
                <num __type="s32">0</num>
              </item>
              <item id="5">
                <num __type="s32">0</num>
              </item>
              <item id="6">
                <num __type="s32">0</num>
              </item>
              <item id="7">
                <num __type="s32">0</num>
              </item>
              <item id="8">
                <num __type="s32">0</num>
              </item>
              <item id="9">
                <num __type="s32">0</num>
              </item>
              <item id="10">
                <num __type="s32">0</num>
              </item>
            </item_list>
          </drop>
        </drop_list>
        <fill_in_category>
          <no_gray_flag_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</no_gray_flag_list>
          <all_yellow_flag_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</all_yellow_flag_list>
          <full_combo_flag_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</full_combo_flag_list>
          <excellent_flag_list __type="s32" __count="16">0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</excellent_flag_list>
        </fill_in_category>
        <daily_bonus_list />
        <ticket_list />
      </player>
    </data>
  </gameend>
</call>
         */
        [HttpPost, Route("8"), XrpcCall("gameend.regist")]
        public ActionResult<EamuseXrpcData> Register([FromBody] EamuseXrpcData data)
        {
            Console.WriteLine(data.Document);
            //TODO: save data

            data.Document = new XDocument(new XElement("response", new XElement("gameend")));

            return data;
        }

        /*
<call model="L44:J:E:A:2018070901" srcid="0120100000706BE5919E" tag="Z34/AEmU">
  <gameend method="final">
    <retry __type="s32">0</retry>
    <pcbinfo client_data_version="0" />
    <data>
      <info>
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <question_list />
      </info>
      <player>
        <end_final_session_id __type="s32">0</end_final_session_id>
        <refid __type="str">DD389C3FFB6F47BA</refid>
        <jid __type="s32">612645048</jid>
        <name __type="str">CLANTEST</name>
        <item>
          <emblem_list __type="s32" __count="96">1 0 0 0 0 0 0 16 0 0 0 0 0 536870916 0 0 65536 0 4194304 0 0 0 268435456 0 65536 8388624 0 0 0 0 0 0 0 147456 0 0 1073741824 4194312 33554432 205520896 0 0 2080 2 0 512 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</emblem_list>
        </item>
        <jbox>
          <point __type="s32">400</point>
          <emblem>
            <type __type="u8">1</type>
            <index __type="s16">1182</index>
          </emblem>
        </jbox>
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <question_list />
      </player>
    </data>
  </gameend>
</call>

<call model="L44:J:E:A:2018070901" srcid="01201000009FDCEA1A6C" tag="dRstALRF">
  <gameend method="final">
    <retry __type="s32">0</retry>
    <pcbinfo client_data_version="0" />
    <data>
      <info>
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <question_list />
      </info>
      <player>
        <end_final_session_id __type="s32">0</end_final_session_id>
        <refid __type="str">DD389C3FFB6F47BA</refid>
        <jid __type="s32">612645048</jid>
        <name __type="str">CLANTEST</name>
        <item>
          <emblem_list __type="s32" __count="96">1 0 0 0 0 0 0 16 0 0 0 0 0 536870916 0 0 65536 0 4194304 0 0 0 268435456 0 65536 8388624 0 0 0 0 0 0 0 147456 0 0 0 4194312 33554432 205520896 0 0 2080 2 0 512 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0</emblem_list>
        </item>
        <jbox>
          <point __type="s32">700</point>
          <emblem>
            <type __type="u8">0</type>
            <index __type="s16">0</index>
          </emblem>
        </jbox>
        <born>
          <status __type="s8">3</status>
          <year __type="s16">2000</year>
        </born>
        <question_list />
      </player>
    </data>
  </gameend>
</call>
         */

        [HttpPost, Route("8"), XrpcCall("gameend.final")]
        public ActionResult<EamuseXrpcData> Final([FromBody] EamuseXrpcData data)
        {
            Console.WriteLine(data.Document);
            //TODO: save data

            data.Document = new XDocument(new XElement("response", new XElement("gameend")));

            return data;
        }
    }
}
